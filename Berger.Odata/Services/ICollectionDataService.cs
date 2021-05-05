using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface ICollectionDataService
    {
        Task<decimal> GetTotalCollectionValue(IList<int> dealerIds);
        Task<decimal> GetTotalCollectionValue(IList<int> dealerIds, DateTime? startDate, DateTime? endDate);
        Task<IList<CollectionDataModel>> GetCustomerCollectionAmount(IList<int> dealerIds, DateTime startDate, DateTime endDate);
    }
}
