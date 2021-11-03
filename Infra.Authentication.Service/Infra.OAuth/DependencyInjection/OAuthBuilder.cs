using Infra.OAuth.Flows.MachineToMachine;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.OAuth.DependencyInjection
{
  internal class OAuthBuilder : IOAuthBuilder
  {
    public IServiceCollection Services { get; }

    public OAuthBuilder(IServiceCollection services)
    {
      Services = services;
    }

    public void AddOAuthM2MAuthFlow()
    {
      Services.AddScoped<IM2MOAuthFlow, M2MOAuthFlow>();
    }
  }
}
