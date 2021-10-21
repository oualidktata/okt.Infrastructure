using System.Collections.Generic;

namespace Infra.oAuthService
{
  public class OAuthSettings : IOAuthSettings
  {
    public string ProviderName { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string Issuer { get; set; }

    public string AuthorizeEndpoint { get; set; }

    public string TokenEndpoint { get; set; }

    public string[] Audiences { get; set; }

    public string Scheme { get; set; }

    public IDictionary<string, string> Scopes => new Dictionary<string, string>()
        {
            {"custom_scope", "custom scope for CC defined in OKTA"},
        };
  }
}
