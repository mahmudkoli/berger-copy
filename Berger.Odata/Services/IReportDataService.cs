using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IReportDataService
    {
        Task<IList<MTDTargetSummaryReportResultModel>> MTDTargetSummary(MTDTargetSummarySearchModel model);
        Task<IList<MTDBrandPerformanceReportResultModel>> MTDBrandPerformance(MTDBrandPerformanceSearchModel model);
    }
}
