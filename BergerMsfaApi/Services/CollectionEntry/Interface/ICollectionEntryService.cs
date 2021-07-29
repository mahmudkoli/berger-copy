using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.Master;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.CollectionEntry.Interface
{
  public  interface ICollectionEntryService
    {
        
        Task<IEnumerable<PaymentModel>> GetCollectionList();
        Task<IEnumerable<AppCollectionEntryModel>> GetAppCollectionListByCurrentUserAsync();
        Task<IEnumerable<CreditControlArea>> GetCreditControlAreaList();
        Task<QueryResultModel<PaymentModel>> GetCollectionByType(CollectionReportSearchModel query);

        Task<PaymentModel> CreateAsync(PaymentModel model);
        Task<PaymentModel> GetCollectionById(int Id);
        Task<PaymentModel> UpdateAsync(Payment model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);
    }
}
