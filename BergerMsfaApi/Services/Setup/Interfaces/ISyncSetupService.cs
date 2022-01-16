using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Sync;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface ISyncSetupService
    {
        Task<SyncSetup> GetById(int id);
        Task<IEnumerable<SyncSetup>> GetAll();
        Task Update(SyncSetup setup);
    }
}
