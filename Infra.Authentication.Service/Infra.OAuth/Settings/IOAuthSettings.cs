using System.Collections.Generic;

namespace Infra.OAuth.Settings
{
  public interface IOAuthSettings
  {
    public string ProviderName { get; }
    
    string Issuer { get; }

    string AuthorizeEndpoint { get; }

    string TokenEndpoint { get; }

    string[] Audiences { get; }

    string ClientId { get; }

    string ClientSecret { get; }

    string Scheme { get; }

    IDictionary<string, string> Scopes { get; }
  }
}
