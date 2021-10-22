using Infra.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infra.Authorization.Policies
{
  public static class TenantPolicy
  {
    public const string Key = "Tenant";

    public static AuthorizationPolicyBuilder AddTenantPolicy(this AuthorizationPolicyBuilder builder, string tenantValue)
    {
      builder.Requirements.Add(new RequireTenant("TenantId", tenantValue));
      return builder;
    }
  }
}
