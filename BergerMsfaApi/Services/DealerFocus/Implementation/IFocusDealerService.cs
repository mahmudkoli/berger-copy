using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Services.DealerFocus.Interfaces;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using BergerMsfaApi.Models.Common;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IFocusDealerService
    {
        #region Focus Dealer
        Task<QueryResultModel<FocusDealerModel>> GetAllFocusDealersAsync(FocusDealerQueryObjectModel query);
        Task<FocusDealerModel> GetFocusDealerById(int id);
        Task<int> CreateFocusDealerAsync(SaveFocusDealerModel model);
        Task<int> UpdateFocusDealerAsync(SaveFocusDealerModel model);
        Task<int> DeleteFocusDealerAsync(int id);
        Task<bool> IsExistFocusDealerAsync(int id);
        #endregion

        #region Dealer
        Task<QueryResultModel<DealerInfoPortalModel>> GetAllDealersAsync(DealerInfoQueryObjectModel query);
        Task<bool> DealerStatusUpdate(DealerInfoStatusModel dealer);
        Task<IList<DealerInfoStatusLogModel>> GetDealerInfoStatusLog(int dealerInfoId);
        #endregion

        #region Excel Dealer Status Update
        Task<DealerStatusExcelExportModel> DealerStatusUpdate(DealerStatusExcelImportModel model);
        #endregion
    }
}
