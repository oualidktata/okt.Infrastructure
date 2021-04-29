using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infra.oAuthService
{
    public class KeyCloackAuthService : ITokenService
    {

        private JwtToken _token = new JwtToken();
        private readonly APIKeySettings _apiKeySettings;
        public KeyCloackAuthService(APIKeySettings settings)
        {
            _apiKeySettings = settings;
        }
        public async Task<string> GetToken()
        {
            if (!_token.IsValidAndNotExpiring)
            {
                _token = await GetNewAccessToken();
            }
            return _token.AccessToken;
        }

        private async Task<JwtToken> GetNewAccessToken()
        {
            //var token = new JwtToken();
            var client = new HttpClient();
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{_apiKeySettings.ClientId}:{_apiKeySettings.ClientSecret}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "client_credentials");
            postMessage.Add("scope", "crm-api-backend");

            var request = new HttpRequestMessage(HttpMethod.Post, _apiKeySettings.TokenUrl)
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _token = JsonConvert.DeserializeObject<JwtToken>(json);
                _token.ExpiresAt = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);
            }
            else
            {
                throw new ApplicationException("Unable to retrieve Access token from Auth server (Okta)");
            }
            return _token;

        }

        private class JwtToken
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }

            [JsonProperty(PropertyName = "expires_in")]
            public int ExpiresIn { get; set; }

            public DateTime ExpiresAt { get; set; }

            public string Scope { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string TokenType { get; set; }

            public bool IsValidAndNotExpiring
            {
                get
                {
                    return !string.IsNullOrEmpty(AccessToken) &&
                        ExpiresAt > DateTime.UtcNow.AddSeconds(30);
                }
            }
        }
    }
}