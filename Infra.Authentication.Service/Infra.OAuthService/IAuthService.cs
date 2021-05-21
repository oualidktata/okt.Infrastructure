using System.Threading.Tasks;

namespace Infra.oAuthService
{
    public interface IAuthService
    {
        Task<string> GetToken();
        IOAuthSettings Settings { get; }
    }
}
