﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AdamMil.Utilities;
using AdamMil.WebDAV.Server;

namespace WebDAV.Server.Tests
{
  #region TypeWithParameters
  public class TypeWithParameters
  {
    public TypeWithParameters(Type type)
    {
      Type = type;
    }

    public static implicit operator TypeWithParameters(Type type)
    {
      return type == null ? null : new TypeWithParameters(type);
    }

    public readonly Dictionary<string, string> Parameters = new Dictionary<string, string>();
    public readonly Type Type;

    public override string ToString()
    {
      return ToString("element", null);
    }

    public string ToString(string elementName, string siteRoot)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append('<').Append(elementName);
      WriteAttributes(sb, siteRoot);
      sb.Append(" />");
      return sb.ToString();
    }

    protected virtual void WriteAttributes(StringBuilder sb, string siteRoot)
    {
      if(Type != null) WriteAttribute(sb, "type", Type.AssemblyQualifiedName);
      foreach(KeyValuePair<string,string> pair in Parameters) WriteAttribute(sb, pair.Key, pair.Value);
    }

    protected static void WriteAttribute(StringBuilder sb, string attributeName, string value)
    {
      if(value != null) sb.Append(' ').Append(attributeName).Append("=\"").Append(XmlUtility.XmlEncode(value)).Append('"');
    }

    protected static void WriteAttribute(StringBuilder sb, string attributeName, bool value)
    {
      sb.Append(' ').Append(attributeName).Append("=\"").Append(value ? "true" : "false").Append('"');
    }

    protected static void WriteAttribute(StringBuilder sb, string attributeName, bool? value)
    {
      if(value.HasValue) WriteAttribute(sb, attributeName, value.Value);
    }
  }
  #endregion

  #region Location
  public class Location : TypeWithParameters
  {
    public Location(string match, Type type, bool enabled) : base(type)
    {
      if(string.IsNullOrEmpty(match)) throw new ArgumentException();
      Match   = match;
      Enabled = enabled;
    }

    public bool? CaseSensitive, ServeRootOptions;
    public readonly bool Enabled;
    public string ID;
    public readonly string Match;

    public override string ToString()
    {
      return ToString(null);
    }

    public string ToString(string siteRoot)
    {
      return ToString("add", siteRoot);
    }

    protected override void WriteAttributes(StringBuilder sb, string siteRoot)
    {
 	    base.WriteAttributes(sb, siteRoot);
      WriteAttribute(sb, "match", Match);
      WriteAttribute(sb, "enabled", Enabled);
      if(!string.IsNullOrEmpty(ID)) WriteAttribute(sb, "id", ID);
      WriteAttribute(sb, "caseSensitive", CaseSensitive);
      WriteAttribute(sb, "serveRootOptions", ServeRootOptions);
    }
  }
  #endregion

  #region FileSystemLocation
  public class FileSystemLocation : Location
  {
    public FileSystemLocation(string match) : this(match, false) { }
    public FileSystemLocation(string match, bool writable) : this(match, "{PhysicalPath}", writable) { }
    public FileSystemLocation(string match, string rootDirectory) : this(match, rootDirectory, false) { }
    public FileSystemLocation(string match, string rootDirectory, bool writable) : base(match, typeof(FileSystemService), true)
    {
      AllowInfinitePropFind = true;
      RootDirectory = rootDirectory;
      Writable      = writable;
    }

    public string RootDirectory;
    public bool AllowInfinitePropFind, Writable;

    protected override void WriteAttributes(StringBuilder sb, string siteRoot)
    {
      base.WriteAttributes(sb, siteRoot);
      string fsRoot = RootDirectory;
      if(fsRoot == "{PhysicalPath}") fsRoot = siteRoot;
      WriteAttribute(sb, "fsRoot", fsRoot);
      WriteAttribute(sb, "writable", Writable);
      WriteAttribute(sb, "allowInfinitePropFind", AllowInfinitePropFind);
    }
  }
  #endregion

  #region WebServer
  public sealed class WebServer : IDisposable
  {
    public WebServer(string serverProgram, int port, string tempDir,
                     TypeWithParameters globalLockManager, TypeWithParameters globalPropertyStore, Location[] locations)
    {
      try
      {
        this.port = port;
        Directory = PrepareServerDirectory(tempDir, port, globalLockManager, globalPropertyStore, locations);
        string args = "/trace:e /systray:false \"/config:" + Path.Combine(Directory, "applicationhost.config") + "\"";
        ProcessStartInfo psi = new ProcessStartInfo(serverProgram, args);
        psi.CreateNoWindow  = true;
        psi.UseShellExecute = false;
        psi.WindowStyle     = ProcessWindowStyle.Hidden;
        process = Process.Start(psi);
      }
      catch
      {
        Dispose();
        throw;
      }
    }

    ~WebServer() { Dispose(); }

    public string Directory { get; private set; }

    public IPEndPoint EndPoint
    {
      get { return new IPEndPoint(IPAddress.Loopback, port); }
    }

    public void CreateDirectory(string name)
    {
      AssertStarted();
      System.IO.Directory.CreateDirectory(Path.Combine(Directory, name));
    }

    public void CreateFile(string name, string textContent)
    {
      CreateFile(name, Encoding.UTF8.GetBytes(textContent));
    }

    public void CreateFile(string name, byte[] content)
    {
      AssertStarted();
      File.WriteAllBytes(Path.Combine(Directory, name), content);
    }

    public void Dispose()
    {
      if(process != null)
      {
        try
        {
          if(!process.HasExited)
          {
            process.Kill();
            process.WaitForExit();
          }
        }
        catch { }
        process = null;
      }

      if(Directory != null)
      {
        try { System.IO.Directory.Delete(Directory, true); }
        catch { }
        Directory = null;
      }

      GC.SuppressFinalize(this);
    }

    void AssertStarted()
    {
      if(process == null || process.HasExited) throw new InvalidOperationException("The server is not running.");
    }

    Process process;
    readonly int port;

    static string CreateTempDirectory(string baseDir)
    {
      const string FileChars = "abcdefghijklmnopqrstuvwxyz0123456789";
      if(string.IsNullOrEmpty(baseDir)) baseDir = Path.GetTempPath();

      Random rand = new Random();
      string prefix = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond).ToStringInvariant() + "_";
      char[] chars = new char[8];
      while(true)
      {
        for(int i=0; i<chars.Length; i++) chars[i] = FileChars[rand.Next(FileChars.Length)];
        string directory = Path.Combine(baseDir, prefix + new string(chars));
        if(!System.IO.Directory.Exists(directory))
        {
          System.IO.Directory.CreateDirectory(directory);
          return directory;
        }
      }
    }

    static string PrepareServerDirectory(string tempDir, int port, TypeWithParameters globalLockManager,
                                         TypeWithParameters globalPropertyStore, Location[] locations)
    {
      string webConfig = File.ReadAllText(Path.Combine(Globals.WebFileDirectory, "web.config"));
      string hostConfig = File.ReadAllText(Path.Combine(Globals.WebFileDirectory, "applicationhost.config"));
      string serverDirectory = CreateTempDirectory(tempDir);

      webConfig = webConfig.Replace(
        "{GlobalLockManager}", globalLockManager == null ? "" : globalLockManager.ToString("davLockManager", serverDirectory));
      webConfig = webConfig.Replace(
        "{GlobalPropertyStore}", globalPropertyStore == null ? "" : globalPropertyStore.ToString("propertyStore", serverDirectory));
      StringBuilder sb = new StringBuilder();
      foreach(Location location in locations) sb.AppendLine(location.ToString(serverDirectory));
      webConfig = webConfig.Replace("{Locations}", sb.ToString());
      webConfig =
        typeNameRe.Replace(webConfig, match => typeof(WebDAVModule).Assembly.GetType(match.Groups[1].Value, true).AssemblyQualifiedName);

      hostConfig = hostConfig.Replace("{PhysicalPath}", serverDirectory).Replace("{Port}", port.ToStringInvariant());
      File.WriteAllText(Path.Combine(serverDirectory, "applicationhost.config"), hostConfig);
      File.WriteAllText(Path.Combine(serverDirectory, "web.config"), webConfig);

      string binDirectory = Path.Combine(serverDirectory, "bin");
      System.IO.Directory.CreateDirectory(binDirectory);
      foreach(string pattern in new string[] { "*.dll", "*.pdb" })
      {
        foreach(string file in System.IO.Directory.GetFiles(Globals.BinaryDirectory, pattern))
        {
          File.Copy(file, Path.Combine(binDirectory, Path.GetFileName(file)));
        }
      }

      return serverDirectory;
    }

    static readonly Regex typeNameRe = new Regex(@"{TypeName:([\w\.]+)}", RegexOptions.Compiled | RegexOptions.ECMAScript);
  }
  #endregion
}