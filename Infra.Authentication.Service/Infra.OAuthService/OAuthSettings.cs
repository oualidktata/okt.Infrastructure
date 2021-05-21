using System.Collections.Generic;

namespace Infra.oAuthService
{
    public class OAuthSettings : IOAuthSettings
    {
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Audiences { get; set; }
        public string Issuer { get; set; }
        public string Scheme { get; set; }

        public string AuthHeaderName { get; set; }

        public IDictionary<string, string> Scopes => new Dictionary<string, string>()
        {
            {"custom_scope", "custom scope for CC defined in OKTA"},
        };

        public string AuthorizationEndpoint { get; set; }

        //public string BasicAuthBase64 => throw new System.NotImplementedException();
    }
}
