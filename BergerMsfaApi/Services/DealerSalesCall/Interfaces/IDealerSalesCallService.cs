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
        Task<DealerSalesCallModel> GetByIdAsync(int id);
        Task<IList<DealerSalesCallModel>> GetAllAsync(int pageIndex, int pageSize);
        Task<SaveDealerSalesCallModel> GetDealerSalesCallByDealerIdAsync(int id);
        Task<IList<SaveDealerSalesCallModel>> GetDealerSalesCallListByDealerIdsAsync(IList<int> ids);
        Task<IList<DealerSalesCallModel>> GetAllByUserIdAsync(int userId);
    }
}
