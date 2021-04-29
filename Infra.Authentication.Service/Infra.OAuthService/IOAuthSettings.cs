using System.Collections.Generic;

namespace Infra.oAuthService
{
    public interface IOAuthSettings
    {
        string[] Audiences { get; }
        string AuthHeaderName { get; }
        string Authorization_Endpoint { get; }
        string BasicAuthBase64 { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string Issuer { get; }
        string SchemeName { get; }
        IDictionary<string, string> Scopes { get; }
        string Token_Endpoint { get; }
        string TokenUrl { get; }
        string TokenAuthenticationScheme {get;}
    }
}