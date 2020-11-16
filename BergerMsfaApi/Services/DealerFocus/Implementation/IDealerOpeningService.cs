using BergerMsfaApi.Controllers.DealerFocus;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IDealerOpeningService
    {
        #region App
        Task<IEnumerable<DealerOpeningModel>> AppGetDealerOpeningListAsync();
        Task<DealerOpeningModel> AppCreateDealerOpeningAsync(DealerOpeningModel model);
        Task<DealerOpeningModel> AppUpdateDealerOpeningAsync(DealerOpeningModel model);
        #endregion

        #region Portal
        Task<IEnumerable<DealerOpeningModel>> GetDealerOpeningListAsync();
     
        Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<DealerOpeningModel> UpdateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<int> DeleteDealerOpeningAsync(int DealerId);
        #endregion

        #region Common
        Task<bool> IsExistAsync(int Id);
        #endregion



    }
}
