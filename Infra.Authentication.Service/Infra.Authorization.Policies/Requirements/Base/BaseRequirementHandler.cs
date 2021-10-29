using System.Threading.Tasks;
using Infra.Features.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.FeatureManagement;

namespace Infra.Authorization.Policies.Requirements.Base
{
  public abstract class BaseRequirementHandler<T> : AuthorizationHandler<T>
    where T : BaseRequirement
  {
    public BaseRequirementHandler(IFeatureManager featureManager)
    {
      FeatureManager = featureManager;
    }

    private IFeatureManager FeatureManager { get; }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
      if (await FeatureDisabled())
      {
        AllowRequirement(context, requirement);
      }
      else
      {
        DefaultHandle(context, requirement);
      }
    }

    private async Task<bool> FeatureDisabled()
    {
      return await FeatureManager.IsEnabledAsync(OAuthFeatureFlags.GenericAuthenticationHandlerDisabled);
    }

    private static void AllowRequirement(AuthorizationHandlerContext context, T requirement)
    {
      context.Succeed(requirement);
    }

    private static void DefaultHandle(AuthorizationHandlerContext context, T requirement)
    {
      if (requirement.IsMet(context.User.Claims))
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }
    }
  }
}
