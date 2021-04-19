﻿using System.Collections.Generic;

namespace Infra.oAuthService
{
    public static class OAuthSettings
    {

        //public static string OAuth2RedirectUrl { get; set; }
        public static string TokenUrl => "/api/oauth/token";
        //public static string AuthorizationUrl  => "/api/oauth/auth";
        //public static string CallBackUrl => "/api/oauth/callback";

        public static string ClientId => "SECRET";
        public static string ClientSecret => "SECRET";

        public static string BasicAuthBase64 => "Basic SECRET==";

        public static string AuthHeaderName => "Authorization";
        public static string ApiBaseUri => "https://localhost:44324/";
        public static string IssuerURI => "https://dev-SECRET.okta.com/oauth2/default";
        public static string[] Audiences => new[] { "api://default" };
        public static string OktaTokenUrl => "https://dev-SECRET.okta.com/oauth2/default/v1/token";

        public static IDictionary<string, string> Scopes => new Dictionary<string, string>() {
                                    { "custom_scope", "custom scope for CC defined in OKTA" },
                                };

        public static string SchemeName => "Bearer";
        public static string ADAuthUri => "https://login.microsoftonline.com/common/oauth2/authorize";
        public static string ADTokenUri => "https://login.microsoftonline.com/common/oauth2/token";

        public const string TokenAuthenticationScheme = "TokenAuthenticationScheme";
    }
}
