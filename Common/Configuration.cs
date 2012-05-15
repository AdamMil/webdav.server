﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using HiA.Configuration;

namespace HiA.WebDAV.Configuration
{

#region AuthorizationFilterCollection
/// <summary>Implements a collection of <see cref="AuthorizationFilterElement"/> objects.</summary>
public sealed class AuthorizationFilterCollection : CustomElementCollection<AuthorizationFilterElement>
{
  /// <inheritdoc/>
  protected override AuthorizationFilterElement CreateElement()
  {
    return new AuthorizationFilterElement();
  }

  /// <inheritdoc/>
  protected override object GetElementKey(AuthorizationFilterElement element)
  {
    return element.Type;
  }
}
#endregion

#region AuthorizationFilterElement
/// <summary>Implements a <see cref="ConfigurationElement"/> representing an <see cref="IAuthorizationFilter"/>.</summary>
public sealed class AuthorizationFilterElement : ConfigurationElement
{
  /// <summary>Initializes a new <see cref="AuthorizationFilterElement"/>.</summary>
  public AuthorizationFilterElement()
  {
    Parameters = new ParameterCollection();
  }

  /// <summary>Gets a collection of additional parameters for the <see cref="IAuthorizationFilter"/>.</summary>
  public ParameterCollection Parameters
  {
    get; private set;
  }

  /// <summary>Gets the type implementing the <see cref="IAuthorizationFilter"/> interface, used restrict requests to resources.</summary>
  [ConfigurationProperty("type"), TypeConverter(typeof(TypeNameConverter)), SubclassTypeValidator(typeof(IAuthorizationFilter))]
  public Type Type
  {
    get { return (Type)this["type"]; }
  }

  /// <inheritdoc/>
  protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
  {
    Parameters.Set(name, value); // save unrecognized attributes so we can pass them as parameters to filter instances
    return true;
  }
}
#endregion

#region LocationCollection
/// <summary>Implements a collection of <see cref="LocationElement"/> objects.</summary>
public sealed class LocationCollection : CustomElementCollection<LocationElement>
{
  /// <inheritdoc/>
  protected override LocationElement CreateElement()
  {
    return new LocationElement();
  }

  /// <inheritdoc/>
  protected override object GetElementKey(LocationElement element)
  {
    return element.Match;
  }
}
#endregion

#region LocationElement
/// <summary>Implements a <see cref="ConfigurationElement"/> representing a location.</summary>
public sealed class LocationElement : ConfigurationElement
{
  /// <summary>Initializes a new <see cref="LocationElement"/>.</summary>
  public LocationElement()
  {
    Parameters = new ParameterCollection();
  }

  /// <summary>Gets a collection of <see cref="AuthorizationFilterElement"/> that represent the authorization filters for the location.</summary>
  [ConfigurationProperty("authorization"), ConfigurationCollection(typeof(AuthorizationFilterCollection))]
  public AuthorizationFilterCollection AuthorizationFilters
  {
    get { return (AuthorizationFilterCollection)this["authorization"]; }
  }

  /// <summary>Gets whether path matching should be sensitive to case.</summary>
  [ConfigurationProperty("caseSensitive", DefaultValue=false), TypeConverter(typeof(BooleanConverter))]
  public bool CaseSensitive
  {
    get { return (bool)this["caseSensitive"]; }
  }

  /// <summary>Gets whether the location should process WebDAV requests.</summary>
  [ConfigurationProperty("enabled", DefaultValue=true), TypeConverter(typeof(BooleanConverter))]
  public bool Enabled
  {
    get { return (bool)this["enabled"]; }
  }

  /// <summary>Gets a string matching request URIs, of the form [[scheme://](hostname|IP)[:port]][/path/]</summary>
  [ConfigurationProperty("match", IsKey=true, IsRequired=true), RegexStringValidator(MatchPattern)]
  public string Match
  {
    get { return (string)this["match"]; }
  }

  /// <summary>Gets a collection of additional parameters for the <see cref="IWebDAVService"/>.</summary>
  public ParameterCollection Parameters
  {
    get; private set;
  }

  /// <summary>Gets the type implementing the <see cref="IWebDAVService"/> interface, used to service requests for the location.</summary>
  [ConfigurationProperty("type"), TypeConverter(typeof(TypeNameConverter)), SubclassTypeValidator(typeof(IWebDAVService))]
  public Type Type
  {
    get { return (Type)this["type"]; }
  }

  /// <inheritdoc/>
  protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
  {
    Parameters.Set(name, value); // save unrecognized attributes so we can pass them as parameters to service instances
    return true;
  }

  internal const string MatchPattern = @"^(?:(?:(?<scheme>[a-zA-Z][a-zA-Z0-9+.\-]*)://)?(?:(?<ipv4>(?:\d{1,3}\.){3}\d{1,3})|(?<hostname>[a-zA-Z0-9](?:[a-zA-Z0-9\-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9\-]*[a-zA-Z0-9]))*)|\[(:ipv6(?:[0-9a-fA-F]{0,4}:){2,7}[0-9a-fA-F]{1,4})\])(?::(?<port>\d{1,5}))?)?(?:/(?<path>[^/]+(?:/[^/]+)*/?)?)?$";
}
#endregion

#region ParameterCollection
/// <summary>Contains additional key/value pairs representing parameters to <see cref="IWebDAVService"/> and
/// <see cref="IAuthorizationFilter"/> objects that were specified in the application configuration file.
/// </summary>
public sealed class ParameterCollection : ReadOnlyDictionaryWrapper<string, string>
{
  internal ParameterCollection() : base(new Dictionary<string,string>()) { }

  internal void Set(string key, string value)
  {
    Items[key] = value;
  }
}
#endregion

#region WebDAVSection
/// <summary>Implements a <see cref="ConfigurationSection"/> that contains the FlairPoint configuration.</summary>
public sealed class WebDAVSection : ConfigurationSection
{
  /// <summary>Gets whether the WebDAV service should be enabled.</summary>
  [ConfigurationProperty("enabled", DefaultValue=true), TypeConverter(typeof(BooleanConverter))]
  public bool Enabled
  {
    get { return (bool)this["enabled"]; }
  }

  /// <summary>Gets a collection of <see cref="LocationElement"/> that represent the configured locations.</summary>
  [ConfigurationProperty("locations"), ConfigurationCollection(typeof(LocationCollection))]
  public LocationCollection Locations
  {
    get { return (LocationCollection)this["locations"]; }
  }

  /// <summary>Gets the <see cref="WebDAVSection"/> containing the WebDAV configuration.</summary>
  public static WebDAVSection Get()
  {
    return ConfigurationManager.GetSection("HiA.WebDAV") as WebDAVSection;
  }
}
#endregion

} // namespace HiA.WebDAV.Configuration