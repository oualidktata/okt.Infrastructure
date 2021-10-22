using Infra.Authorization.Policies.Requirements.Base;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Infra.Authorization.Policies.Requirements
{
  /// <summary>
  /// RequireTenant defined how the requirement is met.
  /// </summary>
  public partial class RequireTenant : BaseRequirement
  {
    public RequireTenant(string claimType, string tenantId)
    {
      ClaimType = claimType;
      TenantId = tenantId;
    }

    private string ClaimType { get; }

    private string TenantId { get; }

    public override bool IsMet(IEnumerable<Claim> claims)
    {
      return claims.Any(t => t.Type == ClaimType && t.Value == TenantId);
    }
  }
}
