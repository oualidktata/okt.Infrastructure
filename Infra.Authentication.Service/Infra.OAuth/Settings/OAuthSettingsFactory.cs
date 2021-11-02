using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra.OAuth.Settings
{
  public class OAuthSettingsFactory : IOAuthSettingsFactory
  {
    public const string DefaultSectionName = "Auth";
    private const string DefaultPkceProviderProperty = "DefaultPkceProvider";
    private const string DefaultMachineToMachineProviderProperty = "DefaultMachineToMachineProvider";

    private string DefaultPkceProvider { get; }

    private string DefaultMachineToMachineProvider { get; }

    private IEnumerable<string> Providers { get; }

    private Dictionary<string, IOAuthSettings> OAuthSettings { get; }

    private IConfiguration Configuration { get; }

    private string SectionName { get; }

    public OAuthSettingsFactory(IConfiguration configuration, string sectionName = DefaultSectionName)
    {
      Configuration = configuration;
      SectionName = sectionName;
      OAuthSettings = new Dictionary<string, IOAuthSettings>();
      Providers = GetProviders();
      DefaultPkceProvider = GetDefaultProvider(DefaultPkceProviderProperty);
      DefaultMachineToMachineProvider = GetDefaultProvider(DefaultMachineToMachineProviderProperty);
    }

    private IEnumerable<string> GetProviders()
    {
      var section = Configuration.GetSection(SectionName);
      var elements = section.AsEnumerable().Select(e => e.Key.Substring(($"{SectionName}").Length));
      var properties = elements.Select(e => e.Split(":")).Where(s => s.Length == 2).Select(e => e.Skip(1).First());
      var providers = properties.Except(new[] { DefaultPkceProviderProperty, DefaultMachineToMachineProviderProperty }).ToList();
      return providers;
    }

    private string GetDefaultProvider(string defaultProviderName)
    {
      return Configuration.GetValue<string>($"{SectionName}:{defaultProviderName}");
    }

    public IOAuthSettings GetDefaultPkce()
    {
      return GetProvider(DefaultPkceProvider);
    }

    public IOAuthSettings GetDefaultMachineToMachine()
    {
      return GetProvider(DefaultMachineToMachineProvider);
    }

    public IOAuthSettings GetProvider(string name)
    {
      if (!Providers.Contains(name))
      {
        throw new ArgumentException("Provider settings isn't defined");
      }

      if (!OAuthSettings.ContainsKey(name))
      {
        var oAuthSetting = new OAuthSettings();
        Configuration.GetSection($"{SectionName}:{name}").Bind(oAuthSetting);
        OAuthSettings[name] = oAuthSetting;
      }

      return OAuthSettings[name];
    }

    public IOAuthSettings GetProviderForIssuer(string issuer)
    {
      foreach (var provider in Providers)
      {
        var settings = GetProvider(provider);
        if (settings.Issuer == issuer)
        {
          return settings;
        }
      }

      throw new Exception($"No provider found for issuer {issuer}");
    }
  }
}
