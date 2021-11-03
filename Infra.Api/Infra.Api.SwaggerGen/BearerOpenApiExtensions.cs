using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infra.Api.SwaggerGen
{
  public static class BearerOpenApiExtensions
  {
    private const string AuthorizeDescription = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"";

    public static void AddBearerAuthentication(this SwaggerGenOptions setup)
    {
      setup.AddSecurityDefinition(
        name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.Http,
          Name = HeaderNames.Authorization,
          In = ParameterLocation.Header,
          Scheme = JwtBearerDefaults.AuthenticationScheme,
          Description = AuthorizeDescription,
        });

      setup.AddSecurityRequirement(CreateOpenApiSecurityRequirement());
    }

    private static OpenApiSecurityRequirement CreateOpenApiSecurityRequirement()
    {
      var openApiSecurityScheme = new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = JwtBearerDefaults.AuthenticationScheme,
        },
      };

      var openApiSecurityRequirement = new OpenApiSecurityRequirement
      {
        { openApiSecurityScheme, new List<string>() }
      };

      return openApiSecurityRequirement;
    }
  }
}
