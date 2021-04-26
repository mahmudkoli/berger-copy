using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IBalanceDataService
    {
        Task<IList<CollectionHistoryResultModel>> GetCollectionHistory(CollectionHistorySearchModel model);
        Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model);
        Task<IList<ChequeBounceResultModel>> GetChequeBounce(ChequeBounceSearchModel model);
        Task<ChequeSummaryResultModel> GetChequeSummary(ChequeSummarySearchModel model);
        Task<CustomerCreditResultModel> GetCustomerCredit(CustomerCreditSearchModel model);
    }
}
