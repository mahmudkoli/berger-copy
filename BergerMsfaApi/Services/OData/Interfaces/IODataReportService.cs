using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Odata.Model;

namespace BergerMsfaApi.Services.OData.Interfaces
{
    public interface IODataReportService
    {
        Task<MySummaryReportResultModel> MySummaryReport(AreaSearchCommonModel area);

        Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model, IList<string> dealerIds);
    }
}
