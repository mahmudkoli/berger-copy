using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IFocusDealerService
    {
        Task<IPagedList<FocusDealerModel>> GetFocusdealerListPaging(int index, int pageSize, string search, string depoId, string[] territories = null, string[] zones = null);
        Task<FocusDealerModel> GetFocusDealerById(int id);
        Task<FocusDealerModel> CreateAsync(FocusDealerModel model);
        Task<FocusDealerModel> UpdateAsync(FocusDealerModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);

        #region Dealer
        public Task<bool> DealerStatusUpdate(DealerInfo dealer);

        public Task<IPagedList<DealerModel>> GetDalerListPaging(int index, int pazeSize, string search, string depoId = null, string[] territories = null, string[] custZones = null, string[] salesGroup = null);
        public Task<IEnumerable<DealerInfoStatusLogModel>> GetDealerInfoStatusLog(int dealerInfoId);
        #endregion

    }
}
