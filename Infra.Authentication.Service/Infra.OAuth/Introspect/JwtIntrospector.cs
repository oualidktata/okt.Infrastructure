using System;
using System.Linq;
using Infra.OAuth.Introspection.Jwt;
using Infra.OAuth.Settings;
using Microsoft.AspNetCore.Http;

namespace Infra.OAuth.Introspection
{
  public class JwtIntrospector : IJwtIntrospector
  {
    private static readonly string[] SupportedAuthorizationSchemes = { "Bearer", "SSWS" };

    private IHttpContextAccessor HttpContextAccessor { get; }

    private IOAuthSettings OAuthSettings { get; }

    public JwtIntrospector(IHttpContextAccessor httpContextAccessor, IOAuthSettingsFactory authSettingsFactory)
    {
      HttpContextAccessor = httpContextAccessor;
      OAuthSettings = authSettingsFactory.GetDefaultPkce();
    }

    public JwtIntrospection GetJwtIntrospection()
    {
      var authorizationHeader = HttpContextAccessor.HttpContext?.GetRequestAuthorizationHeader();
      if (!AuthorizationExists(authorizationHeader))
      {
        throw JwtIntrospectionException.AuthorizationHeaderMissing();
      }

      string accessToken = GetAccessToken(authorizationHeader!);
      return new JwtIntrospection(accessToken, OAuthSettings.ClientId);
    }

    private static bool AuthorizationExists(string? authorizationHeader)
    {
      return !string.IsNullOrEmpty(authorizationHeader);
    }

    private static string GetAccessToken(string authorizationHeader)
    {
      var authorizationScheme = authorizationHeader.ExtractAuthorizationScheme();
      if (!SupportedAuthorizationScheme(authorizationScheme))
      {
        throw JwtIntrospectionException.UnsupportedAuthorizationSchemes(SupportedAuthorizationSchemes);
      }

      return authorizationHeader.ExtractAuthorizationToken(authorizationScheme);
    }

    private static bool SupportedAuthorizationScheme(string authorizationScheme)
    {
      return SupportedAuthorizationSchemes.Contains(authorizationScheme);
    }
  }
}
