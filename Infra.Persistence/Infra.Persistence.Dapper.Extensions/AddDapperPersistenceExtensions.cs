using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Persistence.Dapper.Extensions
{
  public static class AddDapperPersistenceExtensions
  {
    public static IServiceCollection AddDapperPersistence(
      this IServiceCollection serviceCollection,
      string connectionStringName)
    {
      serviceCollection.AddScoped<IDapper>(
        x => new Dapper(
          x.GetRequiredService<IConfiguration>(),
          connectionStringName));

      return serviceCollection;
    }
  }
}
