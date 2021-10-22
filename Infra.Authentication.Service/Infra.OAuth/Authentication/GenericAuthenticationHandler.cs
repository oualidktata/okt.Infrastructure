using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Infra.OAuth;
using Infra.OAuth.Introspection.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Infra.OAuth.Authentication
{
  public class GenericAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
  {
    private IOAuthSettingsFactory AuthSettingsFactory { get; }

    public GenericAuthenticationHandler(
      IOptionsMonitor<TokenAuthenticationOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      IOAuthSettingsFactory authSettingsFactory)
      : base(options, logger, encoder, clock)
    {
      AuthSettingsFactory = authSettingsFactory;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      if (AllowAnonymous())
      {
        return AuthenticateResult.NoResult();
      }

      var token = Request.GetAuthorizationToken();
      return await GenerateAuthenticationResult(token);
    }

    private bool AllowAnonymous()
    {
      var endpoint = Context.GetEndpoint();
      return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
    }

    private async Task<AuthenticateResult> GenerateAuthenticationResult(string token)
    {
      try
      {
        var success = await Authenticate(token);
        return AuthenticateResult.Success(success);
      }
      catch (SecurityTokenValidationException ex)
      {
        return AuthenticateResult.Fail(ex.Message);
      }
    }

    private async Task<AuthenticationTicket> Authenticate(string token)
    {
      var jwt = await ValidateToken(token);
      AuthorizeClaims(jwt.Claims);
      return GenerateAuthenticationTicket(jwt.Claims);
    }

    private async Task<JwtSecurityToken> ValidateToken(string token)
    {
      if (string.IsNullOrEmpty(token))
      {
        throw new SecurityTokenValidationException("Should provide token");
      }

      string issuer = GetTokenIssuer(token);

      var validationParameters = await GetValidationParameters(issuer);
      return ValidationWithParameters(token, validationParameters);
    }

    private string GetTokenIssuer(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jwt = handler.ReadJwtToken(token);
      return jwt.Issuer;
    }

    private async Task<TokenValidationParameters> GetValidationParameters(string issuer)
    {
      var settings = AuthSettingsFactory.GetProviderForIssuer(issuer);

      return TokenValidationParametersBuilder.Create()
        .DefaultClockSkew()
        .ValidateExpiration()
        .ValidateSigninKey(await GetSigningKeys(issuer)) // TODO: Give the possibility to configure how to get signingKey
        .ValidateAudiences(settings.Audiences)
        .ValidateIssuers(settings.Issuer)
        .Build();
    }

    // TODO: Make it possible to Cache the result
    private async Task<IEnumerable<SecurityKey>> GetSigningKeys(string issuer)
    {
      var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                issuer + "/.well-known/oauth-authorization-server",
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());

      var discoveryDocument = await configurationManager.GetConfigurationAsync();

      return discoveryDocument.SigningKeys;
    }

    private static JwtSecurityToken ValidationWithParameters(string token, TokenValidationParameters validationParameters)
    {
      var validatedJwt = ValidateWithJwtSecurityTokenHandler(token, validationParameters);
      EnsureSignedWithSha256(validatedJwt);
      return validatedJwt;
    }

    private static JwtSecurityToken ValidateWithJwtSecurityTokenHandler(string token, TokenValidationParameters validationParameters)
    {
      var handler = new JwtSecurityTokenHandler();
      handler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
      return (JwtSecurityToken)securityToken;
    }

    private static void EnsureSignedWithSha256(JwtSecurityToken validatedJwt)
    {
      // Okta uses RS256
      // https://developer.okta.com/code/dotnet/jwt-validation/#additional-validation-for-access-tokens
      var expectedAlg = SecurityAlgorithms.RsaSha256;
      if (validatedJwt.Header?.Alg == null || validatedJwt.Header?.Alg != expectedAlg)
      {
        throw new SecurityTokenValidationException("The security algorithm must be RsaSha256");
      }
    }

    private void AuthorizeClaims(IEnumerable<Claim> claims)
    {
      var scopes = claims.ExtractScopes();

      // TODO: Give the possibility to be configurable, by adding validation, based on the application
      if (!scopes.Contains("arai"))
      {
        throw new SecurityTokenValidationException("Need \"arai\" scope to be authorized");
      }
    }

    private AuthenticationTicket GenerateAuthenticationTicket(IEnumerable<Claim> tokenClaims)
    {
      var claims = tokenClaims.ToList();

      // TODO: Give possibility to be configurable, by adding claims, based on the applicaiton
      claims.Add(new Claim("TenantId", "pwc"));
      if (claims.Any(c => c.Type == "scp" && c.Value == "admin"))
      {
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
      }
      else
      {
        claims.Add(new Claim(ClaimTypes.Role, "user"));
      }

      var identity = new ClaimsIdentity(claims, Scheme.Name);
      var principal = new ClaimsPrincipal(identity);
      return new AuthenticationTicket(principal, Scheme.Name);
    }
  }
}
