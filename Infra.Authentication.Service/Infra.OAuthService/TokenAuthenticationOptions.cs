using Microsoft.AspNetCore.Authentication;

namespace Infra.oAuthService
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string[] ValidAudiences { get; set; }
        public string ValidIssuer { get; set; }

        public TokenAuthenticationOptions()
        {
        }
    }
}
