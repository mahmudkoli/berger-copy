using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Sync
{
    public interface IApiSyncService
    {
        Task SyncDailySalesNTargetData();
    }
}
