using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IFinancialDataService
    {
        Task<IList<CollectionHistoryResultModel>> GetCollectionHistory(CollectionHistorySearchModel model);
        Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model);
        Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model);
    }
}
