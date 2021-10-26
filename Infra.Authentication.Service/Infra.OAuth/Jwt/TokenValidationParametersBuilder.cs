using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Infra.OAuth.Introspection.Jwt
{
  public class TokenValidationParametersBuilder
  {
    private TokenValidationParameters TokenValidationParameters { get; set; }

    public TokenValidationParametersBuilder()
    {
      TokenValidationParameters = new TokenValidationParameters();
    }

    public static TokenValidationParametersBuilder Create()
    {
      return new TokenValidationParametersBuilder();
    }

    public TokenValidationParametersBuilder DefaultClockSkew()
    {
      // TODO: Give the possibility to configure it
      TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
      return this;
    }

    public TokenValidationParametersBuilder ValidateSigninKey(IEnumerable<SecurityKey> securityKeys)
    {
      TokenValidationParameters.ValidateIssuerSigningKey = true;
      TokenValidationParameters.IssuerSigningKeys = securityKeys;
      TokenValidationParameters.RequireSignedTokens = true;
      return this;
    }

    public TokenValidationParametersBuilder ValidateExpiration()
    {
      // Ensure the token hasn't expired:
      TokenValidationParameters.RequireExpirationTime = true;
      TokenValidationParameters.ValidateLifetime = true;
      return this;
    }

    public TokenValidationParametersBuilder ValidateAudiences(params string[] audiences)
    {
      TokenValidationParameters.ValidateAudience = true;
      TokenValidationParameters.ValidAudiences = audiences;
      return this;
    }

    public TokenValidationParametersBuilder ValidateIssuers(params string[] issuers)
    {
      TokenValidationParameters.ValidateIssuer = true;
      TokenValidationParameters.ValidIssuers = issuers;
      return this;
    }

    public TokenValidationParametersBuilder Customize(Action<TokenValidationParameters> config)
    {
      config(TokenValidationParameters);
      return this;
    }

    public TokenValidationParameters Build()
    {
      // Good practices for JWT token validation
      // https://developer.okta.com/code/dotnet/jwt-validation/#validate-a-token
      var tvp = TokenValidationParameters;
      TokenValidationParameters = new TokenValidationParameters();
      return tvp;
    }
  }
}
