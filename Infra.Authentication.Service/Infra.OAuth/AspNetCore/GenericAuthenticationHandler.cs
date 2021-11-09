using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Infra.OAuth.Authentication;
using Infra.OAuth.Introspection.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infra.OAuth.AspNetCore
{
  public class GenericAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
  {
    private IOAuthAuthentication OAuthAuthentication { get; }

    public GenericAuthenticationHandler(
      IOptionsMonitor<TokenAuthenticationOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      IOAuthAuthentication oAuthAuthentication)
      : base(options, logger, encoder, clock)
    {
      OAuthAuthentication = oAuthAuthentication;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      try
      {
        return await GenerateAuthenticateResult();
      }
      catch (SecurityTokenValidationException ex)
      {
        return AuthenticateResult.Fail(ex.Message);
      }
    }

    private async Task<AuthenticateResult> GenerateAuthenticateResult()
    {
      if (AllowAnonymous(Context))
      {
        return AuthenticateResult.NoResult();
      }

      return AuthenticateResult.Success(await GenerateAuthenticationTicket());
    }

    private bool AllowAnonymous(HttpContext httpContext)
    {
      var endpoint = httpContext.GetEndpoint();
      return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
    }

    private async Task<AuthenticationTicket> GenerateAuthenticationTicket()
    {
      var token = Context.Request.GetAuthorizationToken();
      var claims = await OAuthAuthentication.AuthenticateAsync(token);
      return GenerateAuthenticationTicket(claims);
    }

    private AuthenticationTicket GenerateAuthenticationTicket(IEnumerable<Claim> claims)
    {
      var identity = new ClaimsIdentity(claims, Scheme.Name);
      var principal = new ClaimsPrincipal(identity);
      return new AuthenticationTicket(principal, Scheme.Name);
    }
  }
}
