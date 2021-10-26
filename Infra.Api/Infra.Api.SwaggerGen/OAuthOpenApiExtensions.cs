using System;
using System.Collections.Generic;
using Infra.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infra.Api.SwaggerGen
{
  public static class OAuthOpenApiExtensions
  {
    public static void AddOAuthSecurity(this SwaggerGenOptions setup, OAuthSettings oAuthSettings)
    {
      var flows = new OpenApiOAuthFlows();
      flows.ClientCredentials = new OpenApiOAuthFlow()
      {
        TokenUrl = new Uri(
          oAuthSettings.AuthorizeEndpoint,
          UriKind.Relative),
        Scopes = new Dictionary<string, string>()
        {
          { "custom_scope", "custom scope for CC defined in OKTA" },
        },
      };
      var oauthScheme = new OpenApiSecurityScheme()
      {
        Type = SecuritySchemeType.OAuth2,
        Description = "OAuth2 Description",
        Name = HeaderNames.Authorization,
        In = ParameterLocation.Query,
        Flows = flows,
        Scheme = oAuthSettings.Scheme,
      };

      setup.AddSecurityDefinition(
        oAuthSettings.Scheme,
        oauthScheme);

      var securityrRequirements = new OpenApiSecurityRequirement();
      securityrRequirements.Add(
        oauthScheme,
        new List<string>() { });
      setup.AddSecurityRequirement(securityrRequirements);
    }
  }
}
