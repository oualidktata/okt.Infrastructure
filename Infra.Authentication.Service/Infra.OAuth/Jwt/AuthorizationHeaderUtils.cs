using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Infra.OAuth.Introspection.Jwt
{
  public static class AuthorizationHeaderUtils
  {
    public static string GetAuthorizationToken(this HttpRequest httpRequest)
    {
      var authorizationHeader = httpRequest.GetRequestAuthorizationHeader();
      var scheme = authorizationHeader.ExtractAuthorizationScheme();
      return authorizationHeader.ExtractAuthorizationToken(scheme);
    }

    public static string GetRequestAuthorizationHeader(this HttpContext httpContext)
    {
      return GetRequestAuthorizationHeader(httpContext.Request);
    }

    public static string GetRequestAuthorizationHeader(this HttpRequest httpRequest)
    {
      if (httpRequest.Headers.TryGetValue(HeaderNames.Authorization, out Microsoft.Extensions.Primitives.StringValues value))
      {
        return value.ToString();
      }

      return string.Empty;
    }

    public static string ExtractAuthorizationScheme(this string authorizationHeader)
    {
      var scheme = authorizationHeader.Split(' ')[0];
      return scheme;
    }

    public static string ExtractAuthorizationToken(this string authorizationHeader, string authorizationScheme)
    {
      return authorizationHeader.Substring(authorizationScheme.Length).Trim();
    }
  }
}
