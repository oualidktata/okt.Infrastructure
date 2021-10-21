using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infra.oAuthService
{
  public class M2MOAuthFlowService : IM2MOAuthFlowService
  {
    private AuthenticationToken _token = new AuthenticationToken();
    private IOAuthSettings Settings { get; }

    public M2MOAuthFlowService(IOAuthSettingsFactory authSettingsFactory)
    {
      Settings = authSettingsFactory.GetDefaultMachineToMachine();
    }

    public async Task<string> GetToken()
    {
      if (!_token.IsValidAndNotExpiring)
      {
        _token = await GetNewAccessToken();
      }

      return _token.AccessToken;
    }

    private async Task<AuthenticationToken> GetNewAccessToken()
    {
      var client = new HttpClient();
      var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{Settings.ClientId}:{Settings.ClientSecret}");

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

      var postMessage = new Dictionary<string, string>()
      {
        { "grant_type",  "client_credentials" },
        { "scope", "arai" }
      };

      string requestUrl = Settings.Issuer + Settings.TokenEndpoint;
      var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
      {
        Content = new FormUrlEncodedContent(postMessage)
      };

      var response = await client.SendAsync(request);

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        _token = JsonConvert.DeserializeObject<AuthenticationToken>(json);
        _token.ExpiresAt = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);
      }
      else
      {
        throw new ApplicationException("Unable to retrieve Access token from Auth server (Okta)");
      }

      return _token;
    }
  }
}
