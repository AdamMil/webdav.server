﻿/*
AdamMil.WebDAV.Server is a library providing a flexible, extensible, and fairly
standards-compliant WebDAV server for the .NET Framework.

http://www.adammil.net/
Written 2012-2013 Adam Milazzo

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
using System.Net;

namespace AdamMil.WebDAV.Server
{

#region IWebDAVService
/// <summary>Represents a WebDAV service that serves a subtree within a URL namespace.</summary>
/// <remarks>Before implementing a WebDAV service, it is strongly recommended that be familiar with the WebDAV specification as outlined in
/// RFC 4918 and the HTTP specification as outline in RFC 2616.
/// </remarks>
public interface IWebDAVService
{
  /// <include file="documentation.xml" path="/DAV/IWebDAVService/IsReusable/node()" />
  bool IsReusable { get; }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateAndLock/node()" />
  void CreateAndLock(LockRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateCopyOrMove/node()" />
  CopyOrMoveRequest CreateCopyOrMove(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateDelete/node()" />
  DeleteRequest CreateDelete(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateGetOrHead/node()" />
  GetOrHeadRequest CreateGetOrHead(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateLock/node()" />
  LockRequest CreateLock(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateMkCol/node()" />
  MkColRequest CreateMkCol(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateOptions/node()" />
  OptionsRequest CreateOptions(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePost/node()" />
  PostRequest CreatePost(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePropFind/node()" />
  PropFindRequest CreatePropFind(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePropPatch/node()" />
  PropPatchRequest CreatePropPatch(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePut/node()" />
  PutRequest CreatePut(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateUnlock/node()" />
  UnlockRequest CreateUnlock(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/HandleGenericRequest/node()" />
  bool HandleGenericRequest(WebDAVContext context);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/MakeCollection/node()" />
  void MakeCollection(MkColRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Options/node()" />
  void Options(OptionsRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Post/node()" />
  void Post(PostRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Put/node()" />
  void Put(PutRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/ResolveResource/node()" />
  IWebDAVResource ResolveResource(WebDAVContext context, string resourcePath);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Unlock/node()" />
  void Unlock(UnlockRequest request);
}
#endregion

#region WebDAVService
/// <summary>Provides a base class to simplify the implementation of <see cref="IWebDAVService"/>.</summary>
/// <remarks>Before implementing a WebDAV service, it is strongly recommended that be familiar with the WebDAV specification as outlined in
/// RFC 4918.
/// </remarks>
public abstract class WebDAVService : IWebDAVService
{
  /// <include file="documentation.xml" path="/DAV/IWebDAVService/IsReusable/node()" />
  public abstract bool IsReusable { get; }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateAndLock/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the service does not support the locking of new
  /// resources.
  /// </remarks>
  public virtual void CreateAndLock(LockRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This service does not support the locking of new resources.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateCopyOrMove/node()" />
  /// <remarks>The default implementation returns a new <see cref="CopyOrMoveRequest"/>.</remarks>
  public virtual CopyOrMoveRequest CreateCopyOrMove(WebDAVContext context)
  {
    return new CopyOrMoveRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateDelete/node()" />
  /// <remarks>The default implementation returns a new <see cref="DeleteRequest"/>.</remarks>
  public virtual DeleteRequest CreateDelete(WebDAVContext context)
  {
    return new DeleteRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateGetOrHead/node()" />
  /// <remarks>The default implementation returns a new <see cref="GetOrHeadRequest"/>.</remarks>
  public virtual GetOrHeadRequest CreateGetOrHead(WebDAVContext context)
  {
    return new GetOrHeadRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateLock/node()" />
  /// <remarks>The default implementation returns a new <see cref="LockRequest"/>.</remarks>
  public virtual LockRequest CreateLock(WebDAVContext context)
  {
    return new LockRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateMkCol/node()" />
  /// <remarks>The default implementation returns a new <see cref="MkColRequest"/>.</remarks>
  public virtual MkColRequest CreateMkCol(WebDAVContext context)
  {
    return new MkColRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateOptions/node()" />
  /// <remarks>The default implementation returns a new <see cref="OptionsRequest"/>.</remarks>
  public virtual OptionsRequest CreateOptions(WebDAVContext context)
  {
    return new OptionsRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePost/node()" />
  /// <remarks>The default implementation returns a new <see cref="PostRequest"/>.</remarks>
  public virtual PostRequest CreatePost(WebDAVContext context)
  {
    return new PostRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePropFind/node()" />
  /// <remarks>The default implementation returns a new <see cref="PropFindRequest"/>.</remarks>
  public virtual PropFindRequest CreatePropFind(WebDAVContext context)
  {
    return new PropFindRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePropPatch/node()" />
  /// <remarks>The default implementation returns a new <see cref="PropPatchRequest"/>.</remarks>
  public virtual PropPatchRequest CreatePropPatch(WebDAVContext context)
  {
    return new PropPatchRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreatePut/node()" />
  /// <remarks>The default implementation returns a new <see cref="PutRequest"/>.</remarks>
  public virtual PutRequest CreatePut(WebDAVContext context)
  {
    return new PutRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/CreateUnlock/node()" />
  /// <remarks>The default implementation returns a new <see cref="UnlockRequest"/>.</remarks>
  public virtual UnlockRequest CreateUnlock(WebDAVContext context)
  {
    return new UnlockRequest(context);
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/HandleGenericRequest/node()" />
  /// <remarks>The default implementation does not handle any generic requests.</remarks>
  public virtual bool HandleGenericRequest(WebDAVContext context)
  {
    return false;
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/MakeCollection/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the service does not support the creation of new
  /// collections.
  /// </remarks>
  public virtual void MakeCollection(MkColRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This service does not support the creation of new collections.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Options/node()" />
  public abstract void Options(OptionsRequest request);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Post/node()" />
  /// <remarks>The default implementation responds with 404 Not Found.</remarks>
  public virtual void Post(PostRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = ConditionCodes.NotFound;
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Put/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the service does not support the creation or
  /// alteration of resource entities.
  /// </remarks>
  public virtual void Put(PutRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status =
      new ConditionCode(HttpStatusCode.Forbidden, "This service does not support the creation or alteration of resource entities.");
  }

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/ResolveResource/node()" />
  public abstract IWebDAVResource ResolveResource(WebDAVContext context, string resourcePath);

  /// <include file="documentation.xml" path="/DAV/IWebDAVService/Unlock/node()" />
  /// <remarks>The default implementation responds with 403 Forbidden, indicating that the service does not support the unlocking of
  /// unmapped URLs.
  /// </remarks>
  public virtual void Unlock(UnlockRequest request)
  {
    if(request == null) throw new ArgumentNullException();
    request.Status = new ConditionCode(HttpStatusCode.Forbidden, "This service does not support the unlocking of unmapped URLs.");
  }
}
#endregion

} // namespace AdamMil.WebDAV.Server