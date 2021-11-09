using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Infra.OAuth.Introspection.Jwt;
using Infra.OAuth.Settings;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Infra.OAuth.Authentication
{
  public class OAuthAuthentication : IOAuthAuthentication
  {
    private IOAuthSettingsFactory AuthSettingsFactory { get; }

    public OAuthAuthentication(IOAuthSettingsFactory authSettingsFactory)
    {
      AuthSettingsFactory = authSettingsFactory;
    }

    public async Task<IEnumerable<Claim>> AuthenticateAsync(string token)
    {
      var jwt = await ValidateToken(token);
      AuthorizeClaims(jwt.Claims);
      return ApplyClaimsModifier(jwt.Claims);
    }

    // TODO: Give possibility to be configurable, by adding claims, based on the applicaiton
    private IEnumerable<Claim> ApplyClaimsModifier(IEnumerable<Claim> tokenClaims)
    {
      var claims = tokenClaims.ToList();

      claims.Add(new Claim("TenantId", "pwc"));
      if (claims.Any(c => c.Type == "scp" && c.Value == "admin"))
      {
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
      }
      else
      {
        claims.Add(new Claim(ClaimTypes.Role, "user"));
      }

      return claims;
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
  }
}
