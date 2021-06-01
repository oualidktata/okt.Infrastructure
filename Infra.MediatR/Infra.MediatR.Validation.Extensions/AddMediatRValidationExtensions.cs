using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MediatR.Validation.Extensions
{
  public static class AddMediatRValidationExtensions
  {
    public static IServiceCollection AddMediatRValidation(
      this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient(
        typeof(IPipelineBehavior<,>),
        typeof(ValidationBehavior<,>));
        
      return serviceCollection;
    }
  }
}
