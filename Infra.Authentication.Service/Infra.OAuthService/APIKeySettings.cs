namespace Infra.oAuthService
{
    public class APIKeySettings
    {
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Audiences { get; set; }
        public string Issuer { get; set; }
        public string Scheme { get; set; }


        public string AuthHeaderName { get; set; }
    }
}
