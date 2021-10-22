using Infra.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infra.Authorization.Policies
{
  public static class AdminOnlyPolicy
  {
    public const string Key = "AdminOnly";

    public static AuthorizationPolicyBuilder AddAdminOnlyPolicy(this AuthorizationPolicyBuilder builder)
    {
      builder.Requirements.Add(new RequireAdmin());
      return builder;
    }
  }
}
