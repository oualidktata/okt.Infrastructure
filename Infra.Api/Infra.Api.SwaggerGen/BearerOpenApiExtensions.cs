using System;
using System.Collections.Generic;
using Infra.OAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infra.Api.SwaggerGen
{
  public static class BearerOpenApiExtensions
  {
    public static void AddBearerAuthentication(this SwaggerGenOptions setup)
    {
      setup.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.Http,
          Name = HeaderNames.Authorization,
          In = ParameterLocation.Header,

          // BearerFormat = "JWT",
          Scheme = JwtBearerDefaults.AuthenticationScheme,
          Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        });

      setup.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme,
              },
            },
            new List<string>()
          },
        });
    }

    public static void AddOAuthSecurity(this SwaggerGenOptions setup, IOAuthSettings oauthSettings)
    {
      var flows = new OpenApiOAuthFlows();
      flows.ClientCredentials = new OpenApiOAuthFlow()
      {
        TokenUrl = new Uri(oauthSettings.AuthorizeEndpoint, UriKind.Relative),
        Scopes = oauthSettings.Scopes,
      };
      var oauthScheme = new OpenApiSecurityScheme()
      {
        Type = SecuritySchemeType.OAuth2,
        Description = "OAuth2 Description",
        Name = HeaderNames.Authorization,
        In = ParameterLocation.Query,
        Flows = flows,
        Scheme = oauthSettings.Scheme,
      };

      setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, oauthScheme);

      var securityrRequirements = new OpenApiSecurityRequirement();
      securityrRequirements.Add(oauthScheme, new List<string>() { });
      setup.AddSecurityRequirement(securityrRequirements);
    }
  }
}
