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
        public Task<bool> DealerStatusUpdate(DealerInfo dealer);
        public Task<IPagedList<DealerModel>> GetDalerListPaging(int index, int pazeSize, string search, string depoId = null, string[] territories = null, string[] custZones = null, string[] salesGroup = null);
        public Task<IEnumerable<DealerInfoStatusLogModel>> GetDealerInfoStatusLog(int dealerInfoId);
        #endregion

        #region Excel Dealer Status Update
        public Task<DealerStatusExcelExportModel> DealerStatusUpdate(DealerStatusExcelImportModel model);
        #endregion
    }
}
