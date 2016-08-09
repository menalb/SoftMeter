using SoftMeter.UIConsole.Models;
using System.Threading.Tasks;

namespace SoftMeter.UIConsole.Services
{
    public interface ITopUpSender
    {
        Task TopUpAsync(TopUpRequest request);
    }
}