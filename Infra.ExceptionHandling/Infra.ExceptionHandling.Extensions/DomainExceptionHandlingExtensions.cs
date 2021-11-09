using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.ExceptionHandling.Extensions
{
  public static class DomainExceptionHandlingExtensions
  {
    public static void AddDomainExceptionHandlers(this IServiceCollection services, Assembly assembly)
    {
      var domainExceptionTypes = assembly.GetTypes().Where(t => ImplementsDomainException(t));
      var exceptionHandlerTypes = assembly.GetTypes().Where(t => ImplementsDomainExceptionHandler(t));

      services.AddHandlers(domainExceptionTypes, exceptionHandlerTypes);
    }

    private static bool ImplementsDomainException(Type type) =>
      typeof(DomainException).IsAssignableFrom(type);

    private static bool ImplementsDomainExceptionHandler(this Type type) =>
      type.BaseTypes().Any(t => t.IsParticularGeneric(typeof(DomainExceptionHandler<>)));

    private static void AddHandlers(this IServiceCollection services, IEnumerable<Type> domainExceptionTypes, IEnumerable<Type> exceptionHandlerTypes)
    {
      foreach (var exceptionType in domainExceptionTypes)
      {
        var genericHandlerType = typeof(DomainExceptionHandler<>).MakeGenericType(exceptionType);
        var concreteHandlerType = exceptionHandlerTypes.FirstOrDefault(t => genericHandlerType.IsAssignableFrom(t));

        if (concreteHandlerType != null)
        {
          services.AddSingleton(genericHandlerType, concreteHandlerType);
        }
      }
    }
  }
}
