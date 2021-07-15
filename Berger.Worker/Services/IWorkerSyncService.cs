using System.Threading.Tasks;

namespace Berger.Worker.Services
{
    public interface IWorkerSyncService
    {
        Task SyncDailySalesNTargetData();
    }
}
