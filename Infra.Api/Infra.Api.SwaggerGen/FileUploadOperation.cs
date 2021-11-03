// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// <auto-generated/>

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Troupon.Catalog.Api.DependencyInjectionExtensions
{
  /// <summary>
  /// Filter to enable handling file upload in swagger
  /// Graciosité : https://alexdunn.org/2018/07/12/adding-a-file-upload-field-to-your-swagger-ui-with-swashbuckle/.
  /// </summary>
  public class FileUploadOperation : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (context.MethodInfo == null)
      {
        return;
      }

      if (operation.OperationId?.ToLower() == "UploadDocument".ToLower())
      {
        var props = new Dictionary<string, OpenApiSchema>();
        var schema = new OpenApiSchema() { Type = "string", Format = "binary" };
        props.Add("fileName", new OpenApiSchema
        {
          Type = "file",
          Items = schema,
        });

        operation.RequestBody.Content.Clear();
        operation.RequestBody.Content.Add("application/octet-stream", new OpenApiMediaType()
        {
          Schema = new OpenApiSchema
          {
            Type = "string",
            Format = "binary",
            Properties = props
          },
        });
      }
    }

    private static void ApplySwaggerOperationAttribute(OpenApiOperation operation, IEnumerable<object> actionAttributes)
    {
      var swaggerOperationAttribute = actionAttributes
          .OfType<SwaggerOperationAttribute>()
          .FirstOrDefault();

      if (swaggerOperationAttribute == null)
      {
        return;
      }

      if (swaggerOperationAttribute.Summary != null)
      {
        operation.Summary = swaggerOperationAttribute.Summary;
      }

      if (swaggerOperationAttribute.Description != null)
      {
        operation.Description = swaggerOperationAttribute.Description;
      }

      if (swaggerOperationAttribute.OperationId != null)
      {
        operation.OperationId = swaggerOperationAttribute.OperationId;
      }

      if (swaggerOperationAttribute.Tags != null)
      {
        operation.Tags = swaggerOperationAttribute.Tags
            .Select(tagName => new OpenApiTag { Name = tagName })
            .ToList();
      }
    }

    public static void ApplySwaggerOperationFilterAttributes(
        OpenApiOperation operation,
        OperationFilterContext context,
        IEnumerable<object> actionAndControllerAttributes)
    {
      var swaggerOperationFilterAttributes = actionAndControllerAttributes
          .OfType<SwaggerOperationFilterAttribute>();

      foreach (var swaggerOperationFilterAttribute in swaggerOperationFilterAttributes)
      {
        var filter = (IOperationFilter)Activator.CreateInstance(swaggerOperationFilterAttribute.FilterType);
        filter.Apply(operation, context);
      }
    }

    private void ApplySwaggerResponseAttributes(
        OpenApiOperation operation,
        IEnumerable<object> actionAndControllerAttributes,
        OperationFilterContext context)
    {
      var swaggerResponseAttributes = actionAndControllerAttributes
          .OfType<SwaggerResponseAttribute>();

      foreach (var swaggerResponseAttribute in swaggerResponseAttributes)
      {
        var statusCode = swaggerResponseAttribute.StatusCode.ToString();
        if (!operation.Responses.TryGetValue(statusCode, out OpenApiResponse response))
        {
          response = new OpenApiResponse();
        }

        if (swaggerResponseAttribute.Description != null)
        {
          response.Description = swaggerResponseAttribute.Description;
        }

        operation.Responses[statusCode] = response;
      }
    }
  }
}
