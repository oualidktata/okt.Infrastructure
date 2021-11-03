using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infra.Persistence.SqlServer.Extensions
{
  public static class AddPersistenceExtensions
  {
    public static IServiceCollection AddSqlServerPersistence<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName,
        Assembly runningAssembly)
        where TDbContext : DbContext
    {
      var runningAssemblyName = runningAssembly.GetName().Name;
      return services.AddSqlServerPersistence<TDbContext>(configuration, connectionStringName, runningAssemblyName);
    }

    public static IServiceCollection AddSqlServerPersistence<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName,
        string runningAssembly)
        where TDbContext : DbContext
    {
      var connectionString = configuration.GetConnectionString(connectionStringName);
      services.AddPooledDbContextFactory<TDbContext>((sp, opt) =>
      {
        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        opt.UseLazyLoadingProxies()
           .UseSqlServer(connectionString, b => b.MigrationsAssembly(runningAssembly))
           .UseLoggerFactory(loggerFactory);
      });

      return services;
    }
  }
}
