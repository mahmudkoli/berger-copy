using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.MerchandisingSnapShot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.MerchandisingSnapShot.Interfaces
{
    public interface IMerchandisingSnapShotService
    {
        Task<int> AddAsync(SaveMerchandisingSnapShotModel model);
        Task<bool> AddRangeAsync(List<SaveMerchandisingSnapShotModel> models);
        Task<MerchandisingSnapShotModel> GetByIdAsync(int id);
        Task<QueryResultModel<MerchandisingSnapShotModel>> GetAllAsync(QueryObjectModel query);
        Task<SaveMerchandisingSnapShotModel> GetMerchandisingSnapShotByDealerIdAsync(int id);
        Task<IList<SaveMerchandisingSnapShotModel>> GetMerchandisingSnapShotListByDealerIdsAsync(IList<int> ids);
        Task<IList<AppMerchandisingSnapShotModel>> GetAllByUserIdAsync(int userId);
    }
}
