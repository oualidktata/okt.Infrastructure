using System.Collections.Generic;

namespace Infra.OAuth
{
  public class OAuthSettings : IOAuthSettings
  {
    public string ProviderName { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string AuthorizeEndpoint { get; set; } = string.Empty;

    public string TokenEndpoint { get; set; } = string.Empty;

    public string[] Audiences { get; set; } = new string[0];

    public string Scheme { get; set; } = string.Empty;

    public IDictionary<string, string> Scopes => new Dictionary<string, string>()
        {
            {"custom_scope", "custom scope for CC defined in OKTA"},
        };
  }
}
