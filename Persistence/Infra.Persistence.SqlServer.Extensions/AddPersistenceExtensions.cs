using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infra.Persistence.SqlServer.Extensions
{
    public static class AddPersistenceExtensions
    {
        public static IServiceCollection AddSqlServerPersistence<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName,
            string runningAssembly)
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);
            services.AddPooledDbContextFactory<TDbContext>(
                (
                        serviceProvider,
                        opt) =>
                    opt
                        .UseLazyLoadingProxies()
                        .UseSqlServer(
                            connectionString,
                            b => b.MigrationsAssembly(runningAssembly))
                        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>()));

            return services;
        }
    }
}
