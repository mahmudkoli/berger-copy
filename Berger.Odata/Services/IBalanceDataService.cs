using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IBalanceDataService
    {
        Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model);
        Task<IList<ChequeBounceResultModel>> GetChequeBounce(ChequeBounceSearchModel model);
        Task<ChequeSummaryResultModel> GetChequeSummary(ChequeSummarySearchModel model);
    }
}
