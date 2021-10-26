using System.Threading.Tasks;

namespace Infra.OAuth
{
    public interface IM2MOAuthFlowService
    {
        Task<string> GetToken();
    }
}
