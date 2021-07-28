using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IBalanceDataService
    {
        Task<IList<CollectionHistoryResultModel>> GetMRHistory(CollectionHistorySearchModel model);
        Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model);
        Task<ChecqueBounceResultModel> GetChequeBounce(ChequeBounceSearchModel model);
        Task<ChequeSummaryResultModel> GetChequeSummary(ChequeSummarySearchModel model);
        Task<ChequeSummaryReportResultModel> GetChequeSummaryReport(ChequeSummaryReportSearchModel model);
        Task<CustomerCreditStatusResultModel> GetCustomerCreditStatus(CustomerCreditStatusSearchModel model);
    }
}
