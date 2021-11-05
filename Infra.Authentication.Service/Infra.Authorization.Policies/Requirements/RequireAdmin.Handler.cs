using Infra.Authorization.Policies.Requirements.Base;

namespace Infra.Authorization.Policies.Requirements
{
  /// <summary>
  /// RequireAdmin.Handler is used to apply requirements to authorization context.
  /// </summary>
  public partial class RequireAdmin
  {
    public class Handler : BaseRequirementHandler<RequireAdmin>
    {
    }
  }
}
