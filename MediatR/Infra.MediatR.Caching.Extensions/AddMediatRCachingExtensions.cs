using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MediatR.Caching.Extensions
{
  public static class AddMediatRCachingExtensions
  {
    public static IServiceCollection AddMediatRCaching(
      this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient(
        typeof(IPipelineBehavior<,>),
        typeof(CachingBehavior<,>));

      return serviceCollection;
    }
  }
}
