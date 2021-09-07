using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.ViewModel;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISalesDataService
    {
        #region During dealer visit
        Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model);
        Task<InvoiceDetailsResultModel> GetInvoiceDetails(InvoiceDetailsSearchModel model);
        Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model);
        Task<IList<BrandWisePerformanceResultModel>> GetBrandWisePerformance(BrandWisePerformanceSearchModel model);
        #endregion

        Task<IList<YTDBrandPerformanceSearchModelResultModel>> GetYTDBrandPerformance(YTDBrandPerformanceSearchModelSearchModel model);
        Task<IList<CategoryWiseDealerPerformanceResultModel>> GetCategoryWiseDealerPerformance(CategoryWiseDealerPerformanceSearchModel model);
        Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<string> dealerIds, DealerPerformanceReportType dealerPerformanceReportType);
        Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands);
        Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories);
        Task<int> NoOfBillingDealer(AreaSearchCommonModel area, string division = "", string channel = "");
        Task<IList<TodaysInvoiceValueResultModel>> GetTodaysActivityInvoiceValue(TodaysInvoiceValueSearchModel model, AreaSearchCommonModel area);
        Task<IList<SalesDataModel>> GetMTDActual(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type);
        Task<IList<CustomerDeliveryNoteResultModel>> GetCustomerDeliveryNote(CustomerDeliveryNoteSearchModel model);

        Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> GetReportLastYearAppointedDealerPerformanceSummary(LastYearAppointedDealerPerformanceSearchModel model, List<string> lastYearAppointedDealer);
        Task<IList<RptLastYearAppointDlrPerformanceDetailResultModel>> GetReportLastYearAppointedDealerPerformanceDetail(LastYearAppointedDealerPerformanceSearchModel model, List<string> lastYearAppointedDealer);

        Task<IList<ReportClubSupremePerformance>> GetReportClubSupremePerformance(ClubSupremePerformanceSearchModel model, List<CustNClubMappingVm> clubSupremeDealers, ClubSupremeReportType reportType);

        Task<IList<CustomerPerformanceReport>> GetCustomerWiseRevenue(Expression<Func<CustomerPerformanceReport, CustomerPerformanceReport>> selectProperty, string customerNo, string startDate, string endDate, string division = "-1", List<string> brands = null);
        Task<IList<CustomerPerformanceReport>> GetCbProductCustomerWiseRevenue(string customerNo, string startDate, string endDate, string division = "-1");

        Task<IList<ColorBankPerformanceReport>> GetCbProductReport(Expression<Func<ColorBankPerformanceReport,
                ColorBankPerformanceReport>> selectProperty, string customerNo, string startDate, string endDate, string division = "-1", List<string> brands = null,
            List<string> depots = null,
            List<string> territories = null,
            List<string> salesGroup = null
        );
    }
}
