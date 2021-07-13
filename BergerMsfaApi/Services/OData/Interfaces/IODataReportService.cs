using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Odata.Model;

namespace BergerMsfaApi.Services.OData.Interfaces
{
    public interface IODataReportService
    {
        Task<TodaysActivitySummaryReportResultModel> TodaysActivitySummaryReport(AreaSearchCommonModel area);

        Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model, IList<string> dealerIds);
        Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> ReportLastYearAppointedDealerPerformanceSummary(LastYearAppointedDealerPerformanceSearchModel model);
        Task<IList<RptLastYearAppointDlrPerformanceDetailResultModel>> ReportLastYearAppointedDealerPerformanceDetails(LastYearAppointedDealerPerformanceSearchModel model);
    }
}
