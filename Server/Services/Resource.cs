﻿/*
AdamMil.WebDAV.Server is a library providing a flexible, extensible, and fairly
standards-compliant WebDAV server for the .NET Framework.

http://www.adammil.net/
Written 2012-2015 by Adam Milazzo.

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace AdamMil.WebDAV.Server
{

#region IWebDAVResource
/// <summary>Represents a DAV-compliant resource.</summary>
/// <remarks>Before implementing a WebDAV resource, it is strongly recommended that be familiar with the WebDAV specification as outlined
/// in RFC 4918 and the HTTP specification as outlined in RFCs 7230 through 7235.
/// </remarks>
public interface IWebDAVResource
{
  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/CanonicalPath/node()" />
  string CanonicalPath { get; }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/CopyOrMove/node()" />
  void CopyOrMove(CopyOrMoveRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Delete/node()" />
  void Delete(DeleteRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/GetEntityMetadata/node()" />
  EntityMetadata GetEntityMetadata(bool includeEntityTag);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/GetOrHead/node()" />
  void GetOrHead(GetOrHeadRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/HandleGenericRequest/node()" />
  bool HandleGenericRequest(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Lock/node()" />
  void Lock(LockRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Options/node()" />
  void Options(OptionsRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Post/node()" />
  void Post(PostRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/PropFind/node()" />
  void PropFind(PropFindRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/PropPatch/node()" />
  void PropPatch(PropPatchRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Put/node()" />
  void Put(PutRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/ShouldDenyAccess/node()" />
  bool ShouldDenyAccess(WebDAVContext context, IWebDAVService service, out bool denyExistence);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Unlock/node()" />
  void Unlock(UnlockRequest request);
}
#endregion

#region IStandardResource
/// <summary>Represents a WebDAV resource that uses standard processing for certain requests.</summary>
public interface IStandardResource<T> : IWebDAVResource where T : IStandardResource<T>
{
  /// <summary>Gets whether this resource is a collection resource, which may contain child resources.</summary>
  bool IsCollection { get; }

  /// <summary>Returns the children of this resource (if it's a collection resource), or null if it has no children.</summary>
  IEnumerable<T> GetChildren(WebDAVContext context);

  /// <summary>Returns a dictionary containing the live properties of the resource. The property values should be in the same form as
  /// those given to a <see cref="PropFindRequest"/>, including the ability to include <c>Func&lt;object&gt;</c> delegates to compute
  /// expensive property values on demand. (See
  /// <see cref="PropFindRequest.ProcessStandardRequest(IDictionary{XmlQualifiedName, object})"/> for details.)
  /// </summary>
  /// <remarks>Generally, live properties need not be returned if they are would not be respected by the destination service. For
  /// example, the <c>DAV:lockdiscovery</c>, <c>DAV:supportedlock</c>, and <c>DAV:getetag</c> properties are determined entirely by the
  /// destination resource, and should not be returned (although returning them isn't illegal).
  /// </remarks>
  IDictionary<XmlQualifiedName, object> GetLiveProperties(WebDAVContext context);

  /// <summary>Returns the name of the resource as a minimally escaped path segment that maps to the resource within its parent
  /// collection.
  /// </summary>
  /// <remarks>If a resource has multiple valid names within a collection (e.g. on a case-insensitive service), the name that would be
  /// preferred by humans should be returned, even if it differs from the canonical name used by the service. The name may also differ
  /// depending on the request URI. For example, if a service exposes files in two collections and this resource is mapped to paths
  /// <c>filesById/173</c> and <c>filesByName/hello.txt</c> the name returned would depend on whether the request URL referred to
  /// <c>filesById/</c> or <c>filesByName/</c>. (The name would be "173" in the former case and "hello.txt" in the latter case.) If the
  /// resource is at the root of a DAV service and thus has no name, an empty string must be returned. Otherwise, if the resource is a
  /// child collection, the name should end with a trailing slash.
  /// </remarks>
  string GetMemberName(WebDAVContext context);

  /// <summary>Returns a stream containing the resource's data. This is usually but not necessarily the body that would be returned
  /// from a GET request. For instance, a collection resource might respond with an HTML list of member resources in response to a GET
  /// request, but this HTML listing should not be returned from this method. Instead, if the resource logically has no data stream,
  /// like most collections, this method should return null.
  /// </summary>
  /// <remarks>The stream does not need to be seekable, but a seekable stream is preferred if one can be cheaply obtained. The stream
  /// must be closed by the caller when no longer needed.
  /// </remarks>
  Stream OpenStream(WebDAVContext context);
}
#endregion

#region WebDAVResource
/// <summary>Implements an abstract class to simplify the implementation of <see cref="IWebDAVResource"/>.</summary>
/// <remarks>Before implementing a WebDAV resource, it is strongly recommended that be familiar with the WebDAV specification as outlined
/// in RFC 4918.
/// </remarks>
public abstract class WebDAVResource : IWebDAVResource
{
  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/CanonicalPath/node()" />
  public abstract string CanonicalPath { get; }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/CopyOrMove/node()" />
  public abstract void CopyOrMove(CopyOrMoveRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Delete/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the resource does not support deletion.</remarks>
  public virtual void Delete(DeleteRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This resource does not support deletion.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/GetEntityMetadata/node()" />
  public abstract EntityMetadata GetEntityMetadata(bool includeEntityTag);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/GetOrHead/node()" />
  public abstract void GetOrHead(GetOrHeadRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/HandleGenericRequest/node()" />
  /// <remarks>The default implementation does not handle any generic requests.</remarks>
  public virtual bool HandleGenericRequest(WebDAVContext context)
  {
    return false;
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Lock/node()" />
  /// <remarks>The default implementation responds with 405 Method Not Allowed if locking is not enabled, and 403 Forbidden otherwise,
  /// indicating that the resource cannot be locked.
  /// </remarks>
  public virtual void Lock(LockRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    if(request.Context.LockManager == null) request.Status = ConditionCodes.MethodNotAllowed;
    else request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This resource cannot be locked.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Options/node()" />
  /// <remarks>The default implementation returns options suitable for read-only access to the resource, including the use of <c>GET</c>
  /// and <c>PROPFIND</c> methods, but excluding support for locking or writing.
  /// </remarks>
  public virtual void Options(OptionsRequest request) { }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Post/node()" />
  /// <remarks>The default implementation replies with 405 Method Not Allowed.</remarks>
  public virtual void Post(PostRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = ConditionCodes.MethodNotAllowed;
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/PropFind/node()" />
  public abstract void PropFind(PropFindRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/PropPatch/node()" />
  /// <remarks>The default implementation allows the setting of dead properties outside the <c>DAV:</c> namespace.</remarks>
  public virtual void PropPatch(PropPatchRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.ProcessStandardRequest(CanonicalPath);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Put/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the resource not support setting its content.</remarks>
  public virtual void Put(PutRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This resource does not support setting its content.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/Unlock/node()" />
  /// <remarks>The default implementation responds with 405 Method Not Allowed if locking is not enabled, and 409 Conflict otherwise,
  /// indicating that the resource is not locked.
  /// </remarks>
  public virtual void Unlock(UnlockRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    if(request.Context.LockManager == null) request.Status = ConditionCodes.MethodNotAllowed;
    else request.Status = new ConditionCode(HttpStatusCode.Conflict, "The resource is not locked.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVResource/ShouldDenyAccess/node()" />
  /// <remarks>The default implementation always grants access to the resource.</remarks>
  public virtual bool ShouldDenyAccess(WebDAVContext context, IWebDAVService service, out bool denyExistence)
  {
    denyExistence = false;
    return false;
  }
}
#endregion

} // namespace AdamMil.WebDAV.Server
