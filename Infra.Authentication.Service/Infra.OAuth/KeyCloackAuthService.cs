using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infra.OAuth
{
    //public partial class KeyCloackAuthService : IAuthService
    //{

    //    private AuthenticationToken _token = new AuthenticationToken();
    //    public IOAuthSettings Settings { get; }

    //    public KeyCloackAuthService(OAuthSettings  settings)
    //    {
    //        Settings = settings;
    //    }
    //    public async Task<string> GetToken()
    //    {
    //        if (!_token.IsValidAndNotExpiring)
    //        {
    //            _token = await GetNewAccessToken();
    //        }
    //        return _token.AccessToken;
    //    }

    //    private async Task<AuthenticationToken> GetNewAccessToken()
    //    {
    //        //var token = new JwtToken();
    //        var client = new HttpClient();
    //        var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{Settings.ClientId}:{Settings.ClientSecret}");

    //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

    //        var postMessage = new Dictionary<string, string>();
    //        postMessage.Add("grant_type", "client_credentials");
    //        postMessage.Add("scope", "crm-api-backend");

    //        var request = new HttpRequestMessage(HttpMethod.Post, Settings.TokenUrl)
    //        {
    //            Content = new FormUrlEncodedContent(postMessage)
    //        };

    //        var response = await client.SendAsync(request);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var json = await response.Content.ReadAsStringAsync();
    //            _token = JsonConvert.DeserializeObject<AuthenticationToken>(json);
    //            _token.ExpiresAt = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);
    //        }
    //        else
    //        {
    //            throw new ApplicationException("Unable to retrieve Access token from Auth server (Okta)");
    //        }
    //        return _token;

    //    }
    //}
}
