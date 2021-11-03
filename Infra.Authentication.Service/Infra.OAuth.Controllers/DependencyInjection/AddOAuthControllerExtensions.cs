using Infra.OAuth.Introspection;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.OAuth.Controllers.DependencyInjection
{
  public static class AddOAuthControllerExtensions
  {
    public static IServiceCollection AddOAuthController(this IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.AddScoped<IJwtIntrospector, JwtIntrospector>();

      services.AddControllers()
       .AddApplicationPart(typeof(OAuthController).Assembly)
       .AddControllersAsServices();

      return services;
    }
  }
}
