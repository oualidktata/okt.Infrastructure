using Infra.ExceptionHandling.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
      services.AddSingleton<IGlobalExceptionHandler>(sp =>
        {
          var webHost = sp.GetService<IWebHostEnvironment>();
          return new GlobalExceptionHandler(sp, webHost.IsDevelopment());
        });

      services.AddControllers()
        .AddApplicationPart(typeof(ErrorController).Assembly)
        .AddControllersAsServices();
    }
  }
}
