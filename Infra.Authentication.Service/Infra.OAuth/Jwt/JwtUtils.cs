using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Infra.OAuth.Introspection.Jwt
{
  public static class JwtUtils
  {
    public static bool IsActive(this JwtSecurityToken jwt)
    {
      return jwt.ValidFrom < DateTime.Now && jwt.ValidTo > DateTime.Now;
    }

    public static string ExtractSubjectValue(this JwtSecurityToken jwt)
    {
      return jwt.ExtractClaimValue<string>("sub");
    }

    public static int ExtractExpirationValue(this JwtSecurityToken jwt)
    {
      return jwt.ExtractClaimValue<int>("exp");
    }

    private static T ExtractClaimValue<T>(this JwtSecurityToken jwt, string claimType)
    {
      var value = jwt.Claims.Where(c => c.Type == claimType).First().Value;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    public static IEnumerable<string> ExtractScopes(this IEnumerable<Claim> claims)
    {
      return claims.Where(c => c.Type == "scp").Select(c => c.Value).ToList();
    }
  }
}
