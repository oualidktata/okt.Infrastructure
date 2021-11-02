using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Infra.Api.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Troupon.Catalog.Api.DependencyInjectionExtensions
{
  public static class SwaggerHelper
  {
    public static IServiceCollection AddOpenApi(this IServiceCollection services, Assembly apiAssembly)
    {
      services.AddApiVersioning(cfg =>
      {
        cfg.AssumeDefaultVersionWhenUnspecified = true;
        cfg.DefaultApiVersion = new ApiVersion(1, 0);
      });
      services.AddVersionedApiExplorer();

      services.AddSwaggerGen(setup =>
      {
        var descriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in descriptionProvider.ApiVersionDescriptions)
        {
          ConfigureSwaggerGenPerVersion(setup, description);
        }

        setup.OperationFilter<FileUploadOperation>();
        var xmlCommentsFile = $"{apiAssembly.GetName().Name}.xml";
        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
        setup.IncludeXmlComments(xmlCommentsFullPath);

        setup.DocInclusionPredicate((version, apiDescription) =>
        {
          decimal versionMajor = 1;
          var result = decimal.TryParse(
              version,
              NumberStyles.AllowDecimalPoint,
              CultureInfo.InvariantCulture,
              out versionMajor);
          var major = Math.Truncate(versionMajor);

          var values = apiDescription.RelativePath.Split('/').Skip(2);

          apiDescription.RelativePath = $"api/v{major}/{string.Join("/", values)}";

          var versionParam = apiDescription.ParameterDescriptions.SingleOrDefault(p => p.Name == "version");
          if (versionParam != null)
          {
            apiDescription.ParameterDescriptions.Remove(versionParam);
          }

          return true;
        });

        Auth2FiltersAndSecurity(setup);

        setup.EnableAnnotations();
        setup.IgnoreObsoleteActions();
        setup.OrderActionsBy(descriptor => descriptor.GroupName);
        setup.ResolveConflictingActions(api => api.First());
      });

      return services;
    }

    private static void Auth2FiltersAndSecurity(SwaggerGenOptions setup)
    {
      setup.MapType<FileContentResult>(() => new OpenApiSchema { Type = "string", Format = "binary" });
      setup.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

      setup.AddBearerAuthentication();
    }

    private static void ConfigureSwaggerGenPerVersion(SwaggerGenOptions setup, ApiVersionDescription description)
    {
      setup.SwaggerDoc(description.GroupName, new OpenApiInfo
      {
        Title = $"Troupon.Catalog Api Specification {description.ApiVersion}",
        Description = "Api specification",
        Version = description.ApiVersion.ToString(),
        Contact = new OpenApiContact()
        {
          Email = "oualid.ktata@gmail.com",
          Name = "Oualid Ktata",
        },
        License = new OpenApiLicense()
        {
          Name = "OKT",
          Url = new Uri("https://opensource.org/licenses/MIT"),
        },
      });
    }

    public static void ConfigureSwaggerUI(this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
      app.UseSwaggerUI(setup =>
      {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
          setup.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Troupon Catalog Specification{description.ApiVersion}");
          setup.RoutePrefix = string.Empty;
        }

        setup.RoutePrefix = string.Empty;
      });
    }
  }
}
