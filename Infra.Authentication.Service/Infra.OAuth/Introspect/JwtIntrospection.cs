using Infra.OAuth.Introspection.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Infra.OAuth.Introspection
{
  /// <summary>
  /// Based on OAuth standard https://www.oauth.com/oauth2-servers/token-introspection-endpoint/.
  /// </summary>
  public class JwtIntrospection
  {
    [JsonPropertyName("active")]
    public bool Active { get; }

    [JsonPropertyName("scopes")]
    public string Scopes { get; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; }

    [JsonPropertyName("subject")]
    public string Subject { get; }

    [JsonPropertyName("exp")]
    public int Expiration { get; }

    public JwtIntrospection(string accessToken, string clientId)
    {
      var jwt = ParseToken(accessToken);
      Active = jwt.IsActive();
      Expiration = jwt.ExtractExpirationValue();
      Subject = jwt.ExtractSubjectValue();
      Scopes = FormatScopes(jwt.Claims.ExtractScopes());
      ClientId = clientId;
    }

    private static JwtSecurityToken ParseToken(string accessToken)
    {
      var jwtHandler = new JwtSecurityTokenHandler();
      return jwtHandler.ReadJwtToken(accessToken);
    }

    private static string FormatScopes(IEnumerable<string> scopes)
    {
      return string.Join(" ", scopes);
    }
  }
}
