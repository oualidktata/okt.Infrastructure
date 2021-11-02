using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Api.DependencyInjection
{
  public static class AddPwcApiBehaviourExtentions
  {
    public static IServiceCollection AddPwcApiBehaviour(this IServiceCollection services)
    {
      return services.Configure<ApiBehaviorOptions>(opt => SetupOptions(opt));
    }

    private static void SetupOptions(ApiBehaviorOptions opt)
    {
      opt.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
    }

    private static IActionResult InvalidModelStateResponseFactory(ActionContext actionContext)
    {
      if (actionContext is ActionExecutingContext actionExecutingContext && UnprocessableEntityObject(actionExecutingContext))
      {
        return new UnprocessableEntityObjectResult(actionContext.ModelState);
      }

      return new BadRequestObjectResult(actionContext.ModelState);
    }

    private static bool UnprocessableEntityObject(ActionExecutingContext actionExecutingContext)
    {
      return HasErrors(actionExecutingContext) && HasRightAmountOfArguments(actionExecutingContext);
    }

    private static bool HasErrors(ActionExecutingContext actionExecutingContext)
    {
      return actionExecutingContext.ModelState.ErrorCount > 0;
    }

    private static bool HasRightAmountOfArguments(ActionExecutingContext actionExecutingContext)
    {
      return actionExecutingContext?.ActionArguments.Count == actionExecutingContext.ActionDescriptor.Parameters.Count;
    }
  }
}
