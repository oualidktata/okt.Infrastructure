using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace Infra.OAuth.Authentication
{
  public class TokenAuthenticationOptions : AuthenticationSchemeOptions
  {
    public IEnumerable<string> ValidAudiences { get; set; } = Enumerable.Empty<string>();
    public string ValidIssuer { get; set; } = string.Empty;
  }
}
