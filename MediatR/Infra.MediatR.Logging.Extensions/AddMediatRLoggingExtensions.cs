using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MediatR.Logging.Extensions
{
  public static class AddMediatRLoggingExtensions
  {
    public static IServiceCollection AddMediatRLogging(
      this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient(
        typeof(IPipelineBehavior<,>),
        typeof(LoggingBehavior<,>));
      return serviceCollection;
    }
  }
}
