using Infra.Authorization.Policies.Requirements.Base;

namespace Infra.Authorization.Policies.Requirements
{
  /// <summary>
  /// RequireTenant.Handler is used to apply requirements to authorization context.
  /// </summary>
  public partial class RequireTenant
  {
    public class Handler : BaseRequirementHandler<RequireTenant>
    {
    }
  }
}
