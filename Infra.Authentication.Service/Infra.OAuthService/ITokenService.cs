using System.Threading.Tasks;

namespace Infra.oAuthService
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
}