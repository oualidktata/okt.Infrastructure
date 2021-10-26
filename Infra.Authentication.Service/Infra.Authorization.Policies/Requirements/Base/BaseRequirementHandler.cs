using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Infra.Authorization.Policies.Requirements.Base
{
  public abstract class BaseRequirementHandler<T> : AuthorizationHandler<T>
    where T : BaseRequirement
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
      if (requirement.IsMet(context.User.Claims))
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }

      return Task.CompletedTask;
    }
  }
}
