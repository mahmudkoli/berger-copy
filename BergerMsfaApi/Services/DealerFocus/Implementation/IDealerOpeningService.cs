using Berger.Data.MsfaEntity.DealerFocus;
using BergerMsfaApi.Controllers.DealerFocus;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IDealerOpeningService
    {
        #region App
        Task<IEnumerable<AppDealerOpeningModel>> AppGetDealerOpeningListByCurrentUserAsync();
        Task<DealerOpeningModel> AppCreateDealerOpeningAsync(DealerOpeningModel model);
        Task<DealerOpeningModel> AppUpdateDealerOpeningAsync(DealerOpeningModel model);
        #endregion

        #region Portal
        Task<IPagedList<DealerOpeningModel>> GetDealerOpeningListAsync(int index,int pageSize,string search);
        Task<IPagedList<DealerOpeningModel>> GetDealerOpeningPendingListAsync(int index, int pageSize, string search);
        Task<List<DealerOpening>> GetDealerOpeningPendingListForNotificationAsync();
        Task<bool> ChangeDealerStatus(DealerOpeningStatusChangeModel model);
        Task<DealerOpeningModel> GetDealerOpeningDetailById(int id);
        Task<QueryResultModel<DealerOpeningModel>> GetAllDealersAsync(DealerOpeningQueryObjectModel query);
        Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<DealerOpeningModel> UpdateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<int> DeleteDealerOpeningAsync(int DealerId);
        #endregion

        #region Common
        Task<bool> IsExistAsync(int Id);
        #endregion



    }
}
