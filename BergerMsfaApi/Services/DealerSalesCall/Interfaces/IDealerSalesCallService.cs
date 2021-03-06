using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.DealerSalesCall.Interfaces
{
    public interface IDealerSalesCallService
    {
        Task<int> AddAsync(SaveDealerSalesCallModel model);
        Task<int> UpdateAsync(AppDealerSalesCallModel model);
        Task<bool> AddRangeAsync(List<SaveDealerSalesCallModel> models);
        Task<DealerSalesCallModel> GetByIdAsync(int id);
        Task<QueryResultModel<DealerSalesCallModel>> GetAllAsync(DealerSalesCallQueryObjectModel query);
        Task<SaveDealerSalesCallModel> GetDealerSalesCallByDealerIdAsync(int id);
        Task<IList<SaveDealerSalesCallModel>> GetDealerSalesCallListByDealerIdsAsync(IList<int> ids);
        Task<IList<AppDealerSalesCallModel>> GetAllByUserIdAsync(int userId);
        Task DeleteImage(DealerImageModel dealerImageModel);
    }
}
