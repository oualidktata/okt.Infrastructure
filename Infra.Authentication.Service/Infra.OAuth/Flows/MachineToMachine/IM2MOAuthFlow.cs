using System.Threading.Tasks;

namespace Infra.OAuth.Flows.MachineToMachine
{
    public interface IM2MOAuthFlow
    {
        Task<string> GetToken();
    }
}
