using System;
using System.IO;
using System.Reflection;
using Infra.MediatR.Caching.Extensions;
using Infra.MediatR.Logging.Extensions;
using Infra.MediatR.Validation;
using Infra.MediatR.Validation.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Troupon.Catalog.Core.Application.Behaviors;

namespace Infra.MediatR
{
  public static class AddMediatorExtensions
  {
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
      // Mediator
      services.AddMediatR(assembly);

      services.AddMediatRCaching();
      services.AddMediatRLogging();
      services.AddMediatRValidation();

      // TODO: Is this needed??
      services.AddTransient<TextWriter>(sp => new WrappingWriter(Console.Out));
      return services;
    }
  }
}
