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
        Task<IEnumerable<PaymentModel>> GetCollectionByType(string paymentFrom);

        Task<PaymentModel> CreateAsync(PaymentModel model);
        Task<PaymentModel> UpdateAsync(PaymentModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);
    }
}
