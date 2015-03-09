﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using AdamMil.Tests;
using AdamMil.Utilities;
using AdamMil.WebDAV.Server;
using NUnit.Framework;

namespace AdamMil.WebDAV.Server.Tests
{
  [TestFixture]
  public class PropertyTests : TestBase
  {
    [TestFixtureSetUp]
    public void Setup()
    {
      CreateWebServer(typeof(MemoryLockManager), typeof(MemoryPropertyStore), new FileSystemLocation("/", true) { AllowInfinitePropFind = false });
      fileContent = Server.CreateFile("file", "Herro!");
      Server.CreateDirectory("dir");
      Server.CreateFile("dir/file", "Buhbye!");
    }

    [Test]
    public void T01_BuiltIn()
    {
      TestRequest("PROPFIND", "missing", 404);

      DateTime fileDate = default(DateTime);
      EntityTag etag = null;
      TestRequest("HEAD", "file", null, null, 200, null, response =>
      {
        fileDate = DAVUtility.ParseHttpDate(response.Headers[DAVHeaders.LastModified]);
        etag     = new EntityTag(response.Headers[DAVHeaders.ETag]);
      });

      // test built-in file properties
      Dictionary<XmlQualifiedName, XmlElement> propDict = GetPropertyElementDict("file");
      Assert.AreEqual(fileDate, DAVUtility.GetHttpDate(((DateTimeOffset)XmlUtility.ParseDateTime(propDict[DAVNames.creationdate].InnerText)).UtcDateTime));
      Assert.AreEqual("file", propDict[DAVNames.displayname].InnerText.ToLower());
      Assert.AreEqual(fileContent.Length, int.Parse(propDict[DAVNames.getcontentlength].InnerText, CultureInfo.InvariantCulture));
      Assert.AreEqual("application/octet-stream", propDict[DAVNames.getcontenttype].InnerText);
      if(propDict.ContainsKey(DAVNames.getetag)) Assert.AreEqual(etag, new EntityTag(propDict[DAVNames.getetag].InnerText));
      Assert.AreEqual(fileDate, DAVUtility.GetHttpDate(((DateTimeOffset)XmlUtility.ParseDateTime(propDict[DAVNames.getlastmodified].InnerText)).UtcDateTime));
      Assert.AreEqual(null, propDict[DAVNames.lockdiscovery].FirstChild);
      Assert.AreEqual("", propDict[DAVNames.resourcetype].InnerXml);
      TestHelpers.AssertXmlEquals("<supportedlock xmlns=\"DAV:\"><lockentry><lockscope><exclusive/></lockscope><locktype><write/></locktype></lockentry>" +
                                  "<lockentry><lockscope><shared/></lockscope><locktype><write/></locktype></lockentry></supportedlock>",
                                  propDict[DAVNames.supportedlock]);
      foreach(XmlElement element in propDict.Values) Assert.AreEqual(null, element.GetAttributeValue("xsi:type")); // no types on built-in properties
      propDict = GetPropertyElementDict("file", "<propfind xmlns=\"DAV:\"><prop><getetag/></prop></propfind>");
      Assert.AreEqual(etag, new EntityTag(propDict[DAVNames.getetag].InnerText)); // make sure we get the entity tag if we ask for it specifically

      // test built-in directory properties
      propDict = GetPropertyElementDict("dir");
      Assert.LessOrEqual(fileDate, DAVUtility.GetHttpDate(((DateTimeOffset)XmlUtility.ParseDateTime(propDict[DAVNames.creationdate].InnerText)).UtcDateTime));
      Assert.AreEqual("dir", propDict[DAVNames.displayname].InnerText.ToLower());
      Assert.IsFalse(propDict.ContainsKey(DAVNames.getcontentlength));
      Assert.IsFalse(propDict.ContainsKey(DAVNames.getcontenttype));
      Assert.IsFalse(propDict.ContainsKey(DAVNames.getetag));
      Assert.LessOrEqual(fileDate, DAVUtility.GetHttpDate(((DateTimeOffset)XmlUtility.ParseDateTime(propDict[DAVNames.getlastmodified].InnerText)).UtcDateTime));
      Assert.AreEqual(null, propDict[DAVNames.lockdiscovery].FirstChild);
      TestHelpers.AssertXmlEquals("<resourcetype xmlns=\"DAV:\"><collection/></resourcetype>", propDict[DAVNames.resourcetype]);
      TestHelpers.AssertXmlEquals("<supportedlock xmlns=\"DAV:\"><lockentry><lockscope><exclusive/></lockscope><locktype><write/></locktype></lockentry>" +
                                  "<lockentry><lockscope><shared/></lockscope><locktype><write/></locktype></lockentry></supportedlock>",
                                  propDict[DAVNames.supportedlock]);
      foreach(XmlElement element in propDict.Values) Assert.AreEqual(null, element.GetAttributeValue("xsi:type")); // no types on built-in properties

      // test href and missing properties
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:T=\"TEST:\"><response><href>/dir</href><propstat><prop><resourcetype><collection/></resourcetype></prop><status>HTTP/1.1 200 OK</status></propstat>" +
                                  "<propstat><prop><T:foo /></prop><status>HTTP/1.1 404 Not Found</status></propstat></response></multistatus>",
                                  RequestXml("PROPFIND", "dir", new string[] { DAVHeaders.Depth, "0" }, "<propfind xmlns=\"DAV:\"><prop><resourcetype/><foo xmlns=\"TEST:\"/></prop></propfind>", 207));

      // test <propname> requests
      XmlQualifiedName[] qnames = new[]
      {
        DAVNames.creationdate, DAVNames.displayname, DAVNames.getcontentlength, DAVNames.getcontenttype, DAVNames.getetag,
        DAVNames.getlastmodified, DAVNames.lockdiscovery, DAVNames.resourcetype, DAVNames.supportedlock
      };
      propDict = GetPropertyElementDict("file", "<propfind xmlns=\"DAV:\"><propname/></propfind>");
      Assert.AreEqual(qnames.Length, propDict.Count);
      foreach(XmlQualifiedName qname in qnames) Assert.IsNull(propDict[qname].FirstChild);

      // test recursive, empty <prop> requests
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:T=\"TEST:\"><response><href>/dir/</href><propstat><prop/><status>HTTP/1.1 200 OK</status></propstat></response>" +
                                  "<response><href>/dir/FILE</href><propstat><prop/><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
                                  RequestXml("PROPFIND", "dir/", new string[] { DAVHeaders.Depth, "1" }, "<propfind xmlns=\"DAV:\"><prop/></propfind>", 207));

      // make sure infinite-depth PROPFIND requests are disallowed, as configured
      TestHelpers.AssertXmlEquals("<error xmlns=\"DAV:\"><propfind-finite-depth /></error>",
                                  RequestXml("PROPFIND", "dir", new string[] { DAVHeaders.Depth, "infinity" }, null, 403));

      // test errors in setting properties. (no built-in properties can be set. also test type errors)
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:T=\"TEST:\"><response><href>/file</href><propstat><prop><T:foo/></prop><status>HTTP/1.1 424 Failed Dependency</status></propstat>" +
                                  "<propstat><prop><T:bar/><getlastmodified/></prop><status>HTTP/1.1 422 Unprocessable Entity</status><responsedescription>The value was not formatted correctly for its type.</responsedescription></propstat>" +
                                  "<propstat><prop><getetag /><getcontentlength /></prop><status>HTTP/1.1 403 Forbidden</status><error><cannot-modify-protected-property /></error>" +
                                  "<responsedescription>An attempt was made to set a protected property.</responsedescription></propstat></response></multistatus>",
                                  RequestXml("PROPPATCH", "file", null,
                                             "<propertyupdate xmlns=\"DAV:\" xmlns:T=\"TEST:\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                             "<set><prop><T:foo>bar</T:foo><T:bar xsi:type=\"xs:int\">baz</T:bar><getetag>\"123\"</getetag><getlastmodified>xyz</getlastmodified></prop></set>" +
                                             "<remove><prop><getcontentlength/></prop></remove></propertyupdate>", 207));
    
      // test conditional requests
      TestRequest("PROPFIND", "file", new string[] { DAVHeaders.IfModifiedSince, DAVUtility.GetHttpDateHeader(fileDate) }, 412);
      TestRequest("PROPPATCH", "file", new string[] { DAVHeaders.IfModifiedSince, DAVUtility.GetHttpDateHeader(fileDate) },
                  Encoding.UTF8.GetBytes("<propertyupdate xmlns=\"DAV:\"><set><prop><foo xmlns=\"TEST:\"/></prop></set></propertyupdate>"), 412);
    }

    [Test]
    public void T02_CustomSimple()
    {
      // test parsing of various property types
      TestHelpers.AssertXmlEquals(
          @"<D:multistatus xmlns:D=""DAV:"" xmlns=""TEST:""><D:response><D:href>/file</D:href><D:propstat><D:prop>
          <b64/><bool/><byte/><date/><dateTime/><decimal/><double/><duration1/><duration2/><float/><guid/><hex/><int/><long/><short/><sbyte/><uint/><ulong/><uri/><ushort/>
          </D:prop><D:status>HTTP/1.1 422 Unprocessable Entity</D:status><D:responsedescription>The value was not formatted correctly for its type.</D:responsedescription></D:propstat></D:response></D:multistatus>",
        RequestXml("PROPPATCH", "file", null,
          @"<D:propertyupdate xmlns:D=""DAV:"" xmlns=""TEST:"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><D:set><D:prop>
          <b64 xsi:type=""xs:base64Binary"">SGVsbG8sIH{}dvcmxkIQ==</b64>
          <bool xsi:type=""xs:boolean"">maybe</bool>
          <byte xsi:type=""xs:unsignedByte"">300</byte>
          <date xsi:type=""xs:date"">some day</date>
          <dateTime xsi:type=""xs:dateTime"">some time</dateTime>
          <decimal xsi:type=""xs:decimal"">bazillions</decimal>
          <double xsi:type=""xs:double"">dubba dubba</double>
          <duration1 xsi:type=""xs:duration"">P1Y2MT</duration1>
          <duration2 xsi:type=""xs:duration"">P-1347M</duration2>
          <float xsi:type=""xs:float"">floating up high</float>
          <guid xmlns:ms=""http://microsoft.com/wsdl/types/"" xsi:type=""ms:guid"">046a1990-a762-4157-9636-d340ff0bf01axxx</guid>
          <hex xsi:type=""xs:hexBinary"">48656c6c6f2c20776f726c6421??</hex>
          <int xsi:type=""xs:int"">12345678900</int>
          <long xsi:type=""xs:long"">10223372036854775807</long>
          <short xsi:type=""xs:short"">40000</short>
          <sbyte xsi:type=""xs:byte"">150</sbyte>
          <uint xsi:type=""xs:unsignedInt"">-5</uint>
          <ulong xsi:type=""xs:unsignedLong"">-10</ulong>
          <uri xsi:type=""xs:anyURI"">http://://</uri>
          <ushort xsi:type=""xs:unsignedShort"">90000</ushort>
          </D:prop></D:set></D:propertyupdate>", 207));

      TestHelpers.AssertXmlEquals(
          @"<D:multistatus xmlns:D=""DAV:"" xmlns=""TEST:""><D:response><D:href>/file</D:href><D:propstat><D:prop>
          <b64/><bool/><byte/><custom/><date/><dateTime/><dateTimeZ/><decimal/><double/><duration/><float/><guid/><hex/><inf/><int/><long/><short/><sbyte/><uint/><ulong/><uri/><ushort/>
          </D:prop><D:status>HTTP/1.1 200 OK</D:status></D:propstat></D:response></D:multistatus>",
        RequestXml("PROPPATCH", "file", null,
          @"<propertyupdate xmlns=""DAV:"" xmlns:T=""TEST:"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><set><prop>
          <T:b64 xsi:type=""xs:base64Binary"">SGVsbG8sIHdvcmxkIQ==</T:b64>
          <T:bool xsi:type=""xs:boolean"">true</T:bool>
          <T:byte xsi:type=""xs:unsignedByte"">250</T:byte>
          <T:custom xml:lang=""en"" xmlns:f=""foo:"" xsi:type=""f:bar"">hi</T:custom>
          <T:date xsi:type=""xs:date"">2002-10-10</T:date>
          <T:dateTime xsi:type=""xs:dateTime"">2002-10-10T12:00:00</T:dateTime>
          <T:dateTimeZ xsi:type=""xs:dateTime"">2002-10-10T12:00:00Z</T:dateTimeZ>
          <T:decimal xsi:type=""xs:decimal"">+12678967.543233</T:decimal>
          <T:double xsi:type=""xs:double"">1267.43233E12</T:double>
          <T:duration xsi:type=""xs:duration"">P1Y2MT2H</T:duration>
          <T:float xsi:type=""xs:float"">1267.43233E12</T:float>
          <T:guid xmlns:ms=""http://microsoft.com/wsdl/types/"" xsi:type=""ms:guid"">{046a1990-a762-4157-9636-D340FF0BF01A}</T:guid>
          <T:hex xsi:type=""xs:hexBinary"">48656c6c6f2c20776f726c6421</T:hex>
          <T:inf xsi:type=""xs:double"">INF</T:inf>
          <T:int xsi:type=""xs:int"">1234567890</T:int>
          <T:long xsi:type=""xs:long"">12345678901</T:long>
          <T:short xsi:type=""xs:short"">30000</T:short>
          <T:sbyte xsi:type=""xs:byte"">120</T:sbyte>
          <T:uint xsi:type=""xs:unsignedInt"">3456789012</T:uint>
          <T:ulong xsi:type=""xs:unsignedLong"">10223372036854775807</T:ulong>
          <T:uri xsi:type=""xs:anyURI"">http://www.froogle.com/foo/bar</T:uri>
          <T:ushort xsi:type=""xs:unsignedShort"">60000</T:ushort>
          </prop></set></propertyupdate>", 207));

      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:a=\"TEST:\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"><response><href>/file</href>" +
                                  "<propstat><prop><a:b64 xsi:type=\"xs:base64Binary\">SGVsbG8sIHdvcmxkIQ==</a:b64><a:bool xsi:type=\"xs:boolean\">true</a:bool><a:byte xsi:type=\"xs:unsignedByte\">250</a:byte>" +
                                  "<a:custom xml:lang=\"en\" xmlns:f=\"foo:\" xsi:type=\"f:bar\">hi</a:custom><a:date xsi:type=\"xs:date\">2002-10-10</a:date><a:dateTime xsi:type=\"xs:dateTime\">2002-10-10T12:00:00</a:dateTime>" +
                                  "<a:dateTimeZ xsi:type=\"xs:dateTime\">2002-10-10T12:00:00Z</a:dateTimeZ><a:decimal xsi:type=\"xs:decimal\">12678967.543233</a:decimal>" +
                                  "<a:double xsi:type=\"xs:double\">1.26743233E+15</a:double><a:duration xsi:type=\"xs:duration\">P1Y2MT2H</a:duration><a:float xsi:type=\"xs:float\">1.26743237E+15</a:float>" +
                                  "<a:guid xmlns:ms=\"http://microsoft.com/wsdl/types/\" xsi:type=\"ms:guid\">046a1990-a762-4157-9636-d340ff0bf01a</a:guid>" +
                                  "<a:hex xsi:type=\"xs:hexBinary\">48656C6C6F2C20776F726C6421</a:hex><a:inf xsi:type=\"xs:double\">INF</a:inf><a:int xsi:type=\"xs:int\">1234567890</a:int>" +
                                  "<a:long xsi:type=\"xs:long\">12345678901</a:long><a:short xsi:type=\"xs:short\">30000</a:short><a:sbyte xsi:type=\"xs:byte\">120</a:sbyte>" +
                                  "<a:uint xsi:type=\"xs:unsignedInt\">3456789012</a:uint><a:ulong xsi:type=\"xs:unsignedLong\">10223372036854775807</a:ulong><a:uri xsi:type=\"xs:anyURI\">http://www.froogle.com/foo/bar</a:uri>" +
                                  "<a:ushort xsi:type=\"xs:unsignedShort\">60000</a:ushort></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPFIND", "file", null,
                   "<D:propfind xmlns=\"TEST:\" xmlns:D=\"DAV:\"><D:prop><b64/><bool/><byte/><custom/><date/><dateTime/><dateTimeZ/><decimal/><double/><duration/><float/><guid/><hex/><inf/><int/><long/><short/><sbyte/><uint/><ulong/><uri/><ushort/></D:prop></D:propfind>", 207));

      // test xml:lang inheritence
      TestHelpers.AssertXmlEquals(
          @"<multistatus xmlns=""DAV:"" xmlns:T=""TEST:""><response><href>/file</href><propstat><prop><T:custom/></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "file", null,
          @"<propertyupdate xmlns=""DAV:"" xmlns:T=""TEST:"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><set xml:lang=""es""><prop>
            <T:custom xmlns:f=""foo:"" xsi:type=""f:bar"">bye</T:custom></prop></set></propertyupdate>", 207));

      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:a=\"TEST:\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"><response><href>/file</href>" +
                                  "<propstat><prop><a:custom xml:lang=\"es\" xmlns:f=\"foo:\" xsi:type=\"f:bar\">bye</a:custom></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPFIND", "file", null, "<propfind xmlns:T=\"TEST:\" xmlns=\"DAV:\"><prop><T:custom/></prop></propfind>", 207));

      // test response where the same property both succeeds and fails. (i'm not sure if this is the correct response)
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:a=\"TEST:\"><response><href>/file</href><propstat><prop><a:bool/></prop><status>HTTP/1.1 424 Failed Dependency</status></propstat>" +
                                  "<propstat><prop><a:bool/></prop><status>HTTP/1.1 422 Unprocessable Entity</status><responsedescription>The value was not formatted correctly for its type.</responsedescription></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "file", null,
                   "<propertyupdate xmlns=\"DAV:\" xmlns:T=\"TEST:\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                   "<set><prop><T:bool xsi:type=\"xs:boolean\">false</T:bool></prop></set><set><prop><T:bool xsi:type=\"xs:boolean\">maybe</T:bool></prop></set></propertyupdate>", 207));

      // test processing order and changing the types of properties
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:a=\"TEST:\"><response><href>/file</href><propstat><prop><a:bool /><a:new /><a:int /></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "file", null,
                   "<propertyupdate xmlns=\"DAV:\" xmlns:T=\"TEST:\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                   "<set><prop><T:bool xsi:type=\"xs:boolean\">false</T:bool><T:new>newd</T:new></prop></set>" +
                   "<remove><prop><T:bool/><T:new/><T:int/></prop></remove>" +
                   "<set><prop><T:int>foobar</T:int></prop></set></propertyupdate>", 207));

      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:a=\"TEST:\"><response><href>/file</href>" +
                                  "<propstat><prop><a:int>foobar</a:int></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPFIND", "file", null, "<propfind xmlns:T=\"TEST:\" xmlns=\"DAV:\"><prop><T:int/></prop></propfind>", 207));

      // test removing nonexistent properties. (we give 200 OK rather than an error if the property doesn't exist)
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:a=\"TEST:\"><response><href>/file</href><propstat><prop><a:missing/></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "file", null, "<propertyupdate xmlns=\"DAV:\" xmlns:T=\"TEST:\"><remove><prop><T:missing/></prop></remove></propertyupdate>", 207));
    }

    [Test]
    public void T03_CustomComplex()
    {
      // test complex elements, including changing namespace prefixes witihn xsi:type values
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:T=\"NSA:\"><response><href>/dir</href><propstat><prop><T:custom/></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "dir", null,
          @"<propertyupdate xmlns=""DAV:"" xmlns:A=""NSA:"" xmlns:B=""NSB:"" xmlns:t=""http://www.w3.org/2001/XMLSchema"" xmlns:si=""http://www.w3.org/2001/XMLSchema-instance""><set><prop>
            <A:custom xml:lang=""en"" si:type=""A:complicated"">
              <records>
                <record type=""hairy"" si:type=""A:simple"">big</record>
                <B:customRecord type=""stinky"" xml:lang=""c"" si:type=""t:string"">hold_nose();</B:customRecord>
              </records>
            </A:custom></prop></set></propertyupdate>", 207));

      TestHelpers.AssertXmlEquals(@"<multistatus xmlns=""DAV:"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:X=""NSA:"" xmlns:Y=""NSB:"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""><response><href>/dir</href><propstat><prop>
            <X:custom xml:lang=""en"" xsi:type=""X:complicated"">
              <records>
                <record type=""hairy"" xsi:type=""X:simple"">big</record>
                <Y:customRecord type=""stinky"" xml:lang=""c"" xsi:type=""xs:string"">hold_nose();</Y:customRecord>
              </records>
            </X:custom></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPFIND", "dir", new string[] { DAVHeaders.Depth, "0" }, "<propfind xmlns:T=\"NSA:\" xmlns=\"DAV:\"><prop><T:custom/></prop></propfind>", 207));

      // test that whitespace is preserved
      TestHelpers.AssertXmlEquals("<multistatus xmlns=\"DAV:\" xmlns:T=\"TEST:\"><response><href>/dir</href><propstat><prop><T:custom/></prop><status>HTTP/1.1 200 OK</status></propstat></response></multistatus>",
        RequestXml("PROPPATCH", "dir", null,
          @"<propertyupdate xmlns=""DAV:"" xmlns:T=""TEST:""><set><prop><T:custom>  significant <white>  whitey  </white>  space  </T:custom></prop></set></propertyupdate>", 207));
      TestRequest("PROPFIND", "dir", new string[] { DAVHeaders.Depth, "0" }, null, 207, null, response =>
      {
        using(var reader = new System.IO.StreamReader(response.GetResponseStream()))
        {
          string body = reader.ReadToEnd();
          Assert.IsTrue(body.Contains("custom>  significant <"));
          Assert.IsTrue(body.Contains("white>  whitey  </"));
          Assert.IsTrue(body.Contains("white>  space  </"));
        }
      });
    }

    Dictionary<XmlQualifiedName, XmlElement> GetPropertyElementDict(string requestPath)
    {
      return GetPropertyElementDict(requestPath, null);
    }

    Dictionary<XmlQualifiedName,XmlElement> GetPropertyElementDict(string requestPath, string requestXml)
    {
      Dictionary<XmlQualifiedName, XmlElement> dict = new Dictionary<XmlQualifiedName, XmlElement>();
      foreach(XmlElement element in GetPropertyElements(requestPath, requestXml)) dict[element.GetQualifiedName()] = element;
      return dict;
    }

    byte[] fileContent;
  }
}
