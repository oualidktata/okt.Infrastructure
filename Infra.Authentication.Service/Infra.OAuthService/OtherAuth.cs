namespace Infra.oAuthService
{
    class OtherAuth
    {
        public string ApiBaseUri => "https://localhost:44324/";
        public string ADAuthUri => "https://login.microsoftonline.com/common/oauth2/authorize";
        public string ADTokenUri => "https://login.microsoftonline.com/common/oauth2/token";
    }
}
