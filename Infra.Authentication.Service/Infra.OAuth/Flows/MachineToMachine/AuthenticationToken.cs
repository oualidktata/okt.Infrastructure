using Newtonsoft.Json;
using System;

namespace Infra.OAuth.Flows.MachineToMachine
{
  public class AuthenticationToken
  {
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string Scope { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; } = string.Empty;

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
