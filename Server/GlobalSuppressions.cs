﻿/*
AdamMil.WebDAV.Server is a library providing a flexible, extensible, and fairly
standards-compliant WebDAV server for the .NET Framework.

http://www.adammil.net/
Written 2012-2013 by Adam Milazzo.

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
#if CODE_ANALYSIS // only add these assembly-bloating attributes to builds that will be analyzed
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.ActiveLock.#.ctor(System.String,System.String,AdamMil.WebDAV.Server.LockType,System.Boolean,System.DateTime,System.UInt32,System.Xml.XmlElement)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.ActiveLock.#GetOwnerData()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope="member", Target="AdamMil.WebDAV.Server.ActiveLock.#Save(System.IO.BinaryWriter)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.ActiveLock.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId="dummy", Scope="member", Target="AdamMil.WebDAV.Server.ByteRange.#.ctor(System.Int64,System.Int64,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#BadGateway")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#CannotModifyProtectedProperty")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#Conflict")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#Created")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#FailedDependency")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#Forbidden")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#InsufficientStorage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#Locked")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#LockTokenMatchesRequestUri409")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#LockTokenMatchesRequestUri412")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#MethodNotAllowed")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NoConflictingLock")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NoContent")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NoExternalEntities")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NotAcceptable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NotFound")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NotImplemented")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#NotModified")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#OK")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#PreconditionFailed")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#PreservedLiveProperties")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#PropFindFiniteDepth")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#RequestedRangeNotSatisfiable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#Unauthorized")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ConditionCodes.#UnsupportedMediaType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.DirectoryResource.#Delete(AdamMil.WebDAV.Server.DeleteRequest,System.String,System.IO.DirectoryInfo,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.DirectoryResource.#PropFind(AdamMil.WebDAV.Server.PropFindRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope="member", Target="AdamMil.WebDAV.Server.FileLockManager.#.ctor(AdamMil.WebDAV.Server.Configuration.ParameterCollection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId="timer", Scope="member", Target="AdamMil.WebDAV.Server.FileLockManager.#Dispose(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope="member", Target="AdamMil.WebDAV.Server.FileLockManager.#Dispose(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope="member", Target="AdamMil.WebDAV.Server.FileLockManager.#WriteChanges()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileResource.#GetOrHead(AdamMil.WebDAV.Server.GetOrHeadRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileResource.#PropFind(AdamMil.WebDAV.Server.PropFindRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope="member", Target="AdamMil.WebDAV.Server.FileResource.#Put(AdamMil.WebDAV.Server.PutRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileResource.#Put(AdamMil.WebDAV.Server.PutRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileSystemRootResource.#PropFind(AdamMil.WebDAV.Server.PropFindRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope="member", Target="AdamMil.WebDAV.Server.FileSystemService.#CreateNewFile(AdamMil.WebDAV.Server.WebDAVRequest,System.Action`1<System.IO.Stream>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileSystemService.#CreateNewFile(AdamMil.WebDAV.Server.WebDAVRequest,System.Action`1<System.IO.Stream>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="AdamMil.WebDAV.Server.FileSystemService.#MakeCollection(AdamMil.WebDAV.Server.MkColRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope="member", Target="AdamMil.WebDAV.Server.GetOrHeadRequest.#WriteSimpleIndexHtml(System.Collections.Generic.IEnumerable`1<AdamMil.WebDAV.Server.GetOrHeadRequest+IndexItem>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId="System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope="member", Target="AdamMil.WebDAV.Server.GetOrHeadRequest.#WriteStreamRange(System.IO.Stream,System.Int64,System.Int64)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.GetOrHeadRequest+IndexItem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.GetOrHeadRequest+IndexItem.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.ILockManager.#AddLock(System.String,AdamMil.WebDAV.Server.LockType,System.Boolean,System.Nullable`1<System.UInt32>,System.Xml.XmlElement)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="1#", Scope="member", Target="AdamMil.WebDAV.Server.ISupportAuthorization.#ShouldDenyAccess(AdamMil.WebDAV.Server.WebDAVContext,System.Boolean&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.LockRequest.#OwnerData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.LockTokenSubmittedConditionCode.#.ctor(System.String[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.LockType.#ExclusiveWrite")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.LockType.#SharedWrite")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.LockType.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.LockType.#WriteLocks")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.OptionsRequest+AllowedMethodCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.OptionsRequest+SupportedExtensionCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope="type", Target="AdamMil.WebDAV.Server.PropertyNameSet")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope="type", Target="AdamMil.WebDAV.Server.PropertyPatch+PropertyPatchSet")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.PropertyPatch+PropertyPatchSet")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.PropertyPatch+PropertyRemovalSet")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope="type", Target="AdamMil.WebDAV.Server.PropertyPatch+PropertyRemovalSet")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.PropertyPatchValue.#Element")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.PropertyPatchValue.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId="Flags", Scope="type", Target="AdamMil.WebDAV.Server.PropFindFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId="Flags", Scope="member", Target="AdamMil.WebDAV.Server.PropFindRequest.#Flags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope="member", Target="AdamMil.WebDAV.Server.PropFindRequest.#ProcessStandardRequest`1(!!0,System.Func`2<!!0,System.String>,System.Func`2<!!0,System.Collections.Generic.IDictionary`2<System.Xml.XmlQualifiedName,AdamMil.WebDAV.Server.PropFindValue>>,System.Func`2<!!0,System.Collections.Generic.IEnumerable`1<!!0>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope="member", Target="AdamMil.WebDAV.Server.PropFindRequest.#ProcessStandardRequest`1(!!0,System.Func`2<!!0,System.String>,System.Func`2<!!0,System.Collections.Generic.IDictionary`2<System.Xml.XmlQualifiedName,System.Object>>,System.Func`2<!!0,System.Collections.Generic.IEnumerable`1<!!0>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope="member", Target="AdamMil.WebDAV.Server.PropFindRequest.#WriteResponse()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.PropFindRequest+PropFindResourceCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope="member", Target="AdamMil.WebDAV.Server.PropFindResource.#SetValueCore(System.Xml.XmlQualifiedName,System.Object,System.Xml.XmlQualifiedName,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope="member", Target="AdamMil.WebDAV.Server.PropFindResource.#ValidateValueType(System.Xml.XmlQualifiedName,System.Object,System.Xml.XmlQualifiedName)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.PropFindValue.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.PropPatchRequest+PropertyPatchCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope="member", Target="AdamMil.WebDAV.Server.ResourceType.#Collection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId="System.Xml.XmlNode", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVContext.#LoadRequestXml()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVContext.#OpenRequestXml()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVContext.#OpenResponseBody(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope="type", Target="AdamMil.WebDAV.Server.WebDAVContext+Configuration")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope="type", Target="AdamMil.WebDAV.Server.WebDAVContext+Configuration")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVException.#.ctor(AdamMil.WebDAV.Server.ConditionCode,System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="3#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="4#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="3#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&,System.String&,System.String&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="4#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&,System.String&,System.String&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="6#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&,System.String&,System.String&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId="5#", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule.#ResolveUri(AdamMil.WebDAV.Server.WebDAVContext,System.Uri,System.Boolean,AdamMil.WebDAV.Server.IWebDAVService&,AdamMil.WebDAV.Server.IWebDAVResource&,System.String&,System.String&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope="member", Target="AdamMil.WebDAV.Server.WebDAVModule+LocationConfig.#ParseMatch(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.Configuration.AuthorizationFilterElement.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.Configuration.LocationCollection.#GetElementKey(AdamMil.WebDAV.Server.Configuration.LocationElement)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.Configuration.LocationElement.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope="member", Target="AdamMil.WebDAV.Server.Configuration.LockManagerElement.#Type")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope="type", Target="AdamMil.WebDAV.Server.Configuration.ParameterCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope="type", Target="AdamMil.WebDAV.Server.Configuration.ParameterCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.CopyOrMoveRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.GetOrHeadRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.LockRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.OptionsRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.PutRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0", Scope="member", Target="AdamMil.WebDAV.Server.UnlockRequest.#.ctor(AdamMil.WebDAV.Server.WebDAVContext)")]

#endif