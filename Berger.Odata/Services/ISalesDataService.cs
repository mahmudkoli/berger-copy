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

        Task<IList<SalesDataModel>>  GetMyTargetSales(DateTime fromDate, DateTime endDate, string division, EnumVolumeOrValue volumeOrValue,
            MyTargetReportType targetReportType, IList<string> dealerIds, EnumMyTargetBrandType brandType);
        Task<IList<TotalInvoiceValueResultModel>> GetReportTotalInvoiceValue(TotalInvoiceValueSearchModel model, IList<string> dealerIds);
        Task<IList<BrandOrDivisionWisePerformanceResultModel>> GetReportBrandOrDivisionWisePerformance(BrandOrDivisionWisePerformanceSearchModel model, IList<string> dealerIds);
        Task<IList<DealerPerformanceResultModel>> GetReportDealerPerformance(DealerPerformanceSearchModel model, IList<string> dealerIds);
        Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<string> dealerIds, DealerPerformanceReportType dealerPerformanceReportType);
        Task<int> NoOfBillingDealer(IList<string> dealerIds);
        Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands);
        Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones);
        Task<int> NoOfBillingDealer(AreaSearchCommonModel area, string division = "", string channel = "");
        Task<IList<TotalInvoiceValueResultModel>> GetTotalInvoiceValue(TotalInvoiceValueSearchModel model, AreaSearchCommonModel area);
    }
}
