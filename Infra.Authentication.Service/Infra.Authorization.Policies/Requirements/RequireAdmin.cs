using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infra.Authorization.Policies.Requirements.Base;

namespace Infra.Authorization.Policies.Requirements
{
  /// <summary>
  /// RequireAdmin defined how the requirement is met.
  /// </summary>
  public partial class RequireAdmin : BaseRequirement
  {
    public RequireAdmin()
    {
    }

    private string ClaimType { get; } = "scp";

    private string ClaimValue { get; } = "admin";

    public override bool IsMet(IEnumerable<Claim> claims)
    {
      return claims.Any(t => t.Type == ClaimType && t.Value == ClaimValue);
    }
  }
}
