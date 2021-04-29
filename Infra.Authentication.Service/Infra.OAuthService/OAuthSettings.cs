using System.Collections.Generic;

namespace Infra.oAuthService
{
    public class OAuthSettings : IOAuthSettings
    {

        //public static string OAuth2RedirectUrl { get; set; }
        public string TokenUrl => "/api/oauth/token";
        //public static string AuthorizationUrl  => "/api/oauth/auth";
        //public static string CallBackUrl => "/api/oauth/callback";

        public string ClientId => "SECRET";
        public string ClientSecret => "SECRET";

        public string BasicAuthBase64 => "Basic SECRET==";

        public string AuthHeaderName => "Authorization";

        public string Issuer => "https://dev-SECRET.okta.com/oauth2/default";
        public string[] Audiences => new[] { "api://default" };

        public string Authorization_Endpoint => "";
        public string Token_Endpoint => "https://dev-SECRET.okta.com/oauth2/default/v1/token";

        public IDictionary<string, string> Scopes => new Dictionary<string, string>() {
                                    { "custom_scope", "custom scope for CC defined in OKTA" },
                                };

        public string SchemeName => "Bearer";
      
        public string TokenAuthenticationScheme => "TokenAuthenticationScheme";
    }
}
