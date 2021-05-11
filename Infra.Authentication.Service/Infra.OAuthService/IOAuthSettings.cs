using System.Collections.Generic;

namespace Infra.oAuthService
{
    public interface IOAuthSettings
    {
        string[] Audiences { get; }
        string AuthHeaderName { get; }
        string AuthorizationEndpoint { get; }
        //string BasicAuthBase64 { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string Issuer { get; }
        string Scheme { get; }
        IDictionary<string, string> Scopes { get; }
        string TokenUrl { get; }
    }
}