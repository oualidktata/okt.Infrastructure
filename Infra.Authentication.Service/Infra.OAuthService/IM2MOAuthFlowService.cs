using System.Threading.Tasks;

namespace Infra.oAuthService
{
    public interface IM2MOAuthFlowService
    {
        Task<string> GetToken();
    }
}
