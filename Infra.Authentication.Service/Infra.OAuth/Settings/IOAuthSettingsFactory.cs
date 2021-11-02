namespace Infra.OAuth.Settings
{
  public interface IOAuthSettingsFactory
  {
    IOAuthSettings GetDefaultPkce();

    IOAuthSettings GetDefaultMachineToMachine();

    IOAuthSettings GetProvider(string name);

    IOAuthSettings GetProviderForIssuer(string issuer);
  }
}
