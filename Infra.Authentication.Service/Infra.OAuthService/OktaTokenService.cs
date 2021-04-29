using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infra.oAuthService
{
    public partial class OktaTokenService : ITokenService
    {

        private OktaToken _token = new OktaToken();
        private readonly APIKeySettings _oktaSettings;
        public OktaTokenService(APIKeySettings settings)
        {
            _oktaSettings = settings;
        }
        public async Task<string> GetToken()
        {
            if (!_token.IsValidAndNotExpiring)
            {
                _token = await GetNewAccessToken();
            }
            return _token.AccessToken;
        }

        private async Task<OktaToken> GetNewAccessToken()
        {
            var token = new OktaToken();
            var client = new HttpClient();
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{_oktaSettings.ClientId}:{_oktaSettings.ClientSecret}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "client_credentials");
            postMessage.Add("scope", "custom_scope");

            var request = new HttpRequestMessage(HttpMethod.Post, _oktaSettings.TokenUrl)
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _token = JsonConvert.DeserializeObject<OktaToken>(json);
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