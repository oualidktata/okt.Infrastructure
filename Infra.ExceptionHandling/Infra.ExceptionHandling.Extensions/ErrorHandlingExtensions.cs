using Infra.ExceptionHandling.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.ExceptionHandling.Extensions
{
  public static class ErrorHandlingExtensions
  {
    public static void UseErrorHandling(this IApplicationBuilder app)
    {
      app.UseExceptionHandler("/Error");
    }

    public static void AddWebExceptionHandler(this IServiceCollection services)
    {
      services.AddSingleton<IGenericExceptionHandler, GenericWebExceptionHandler>();
      services.AddControllers()
       .AddApplicationPart(typeof(ErrorController).Assembly)
       .AddControllersAsServices();
    }
  }
}
