using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISalesDataService
    {
        #region During dealer visit
        Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model);
        Task<InvoiceDetailsResultModel> GetInvoiceDetails(InvoiceDetailsSearchModel model);
        Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model);
        Task<IList<BrandOrDivisionWiseMTDResultModel>> GetBrandOrDivisionWisePerformance(BrandOrDivisionWiseMTDSearchModel model);
        #endregion

        Task<IList<YTDBrandPerformanceSearchModelResultModel>> GetYTDBrandPerformance(YTDBrandPerformanceSearchModelSearchModel model);
        Task<IList<CategoryWiseDealerPerformanceResultModel>> GetCategoryWiseDealerPerformance(CategoryWiseDealerPerformanceSearchModel model);
        Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<string> dealerIds, DealerPerformanceReportType dealerPerformanceReportType);
        Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands);
        Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones);
        Task<int> NoOfBillingDealer(AreaSearchCommonModel area, string division = "", string channel = "");
        Task<IList<TodaysInvoiceValueResultModel>> GetTodaysActivityInvoiceValue(TodaysInvoiceValueSearchModel model, AreaSearchCommonModel area);
        Task<IList<SalesDataModel>> GetMTDActual(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type);
    }
}
