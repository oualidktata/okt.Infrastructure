using System;
using System.Linq;
using System.Reflection;
using Infra.Common.Models;
using Infra.Persistence.EntityFramework.Repositories;
using Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Persistence.EntityFramework.Extensions
{
  public static class AddEfRepositoryExtensions
  {
    public static IServiceCollection AddEfReadRepository<TDbContext>(
      this IServiceCollection serviceCollection)
      where TDbContext : DbContext
    {
      RegisterRepo<TDbContext>(
        serviceCollection,
        typeof(BaseEntity),
        typeof(IReadRepository<>),
        typeof(EfReadRepository<,>));

      return serviceCollection;
    }

    public static IServiceCollection AddEfWriteRepository<TDbContext>(
      this IServiceCollection serviceCollection)
      where TDbContext : DbContext
    {
      RegisterRepo<TDbContext>(
        serviceCollection,
        typeof(BaseEntity),
        typeof(IWriteRepository<>),
        typeof(EfWriteRepository<,>));

      return serviceCollection;
    }

    private static void RegisterRepo<TDbContext>(
      IServiceCollection services,
      Type baseEntityType,
      Type repoInterfaceType,
      Type repoConcreteType)
      where TDbContext : DbContext
    {
      var allAssembly = Assembly.GetAssembly(typeof(TDbContext))
        ?.GetReferencedAssemblies();

      if (allAssembly == null) return;

      foreach (var assemblyName in allAssembly)
      {
        var assembly = Assembly.Load(assemblyName);

        foreach (var type in assembly.GetTypes()
          .Where(
            t => t.IsClass
                 && !t.IsAbstract
                 && t.IsAssignableTo(baseEntityType)
                 && t.IsAssignableTo(typeof(IRepoQueryable))))
        {
          var interfaceType = repoInterfaceType.MakeGenericType(type);
          var concreteType = repoConcreteType.MakeGenericType(
            type,
            typeof(TDbContext));

          services.AddTransient(
            interfaceType,
            concreteType);
        }
      }
    }
  }
}
