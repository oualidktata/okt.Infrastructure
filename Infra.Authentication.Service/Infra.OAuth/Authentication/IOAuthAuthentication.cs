using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infra.OAuth.Authentication
{
  public interface IOAuthAuthentication
  {
    Task<IEnumerable<Claim>> AuthenticateAsync(string token);
  }
}
