using Berger.Data.MsfaEntity.Master;
using BergerMsfaApi.Models.CollectionEntry;
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
        Task<IEnumerable<PaymentModel>> GetCollectionByType(int customerTypeId);

        Task<PaymentModel> CreateAsync(PaymentModel model);
        Task<PaymentModel> UpdateAsync(PaymentModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);
    }
}
