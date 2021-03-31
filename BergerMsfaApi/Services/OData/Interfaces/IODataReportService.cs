using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace BergerMsfaApi.Services.OData.Interfaces
{
    public interface IODataReportService
    {
        Task<MySummaryReportResultModel> MySummaryReport();

        Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model);
    }
}
