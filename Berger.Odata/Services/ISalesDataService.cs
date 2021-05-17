﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            MyTargetReportType targetReportType, IList<int> dealerIds, EnumMyTargetBrandType brandType);
        Task<IList<TotalInvoiceValueResultModel>> GetReportTotalInvoiceValue(TotalInvoiceValueSearchModel model, IList<int> dealerIds);
        Task<IList<BrandOrDivisionWisePerformanceResultModel>> GetReportBrandOrDivisionWisePerformance(BrandOrDivisionWisePerformanceSearchModel model, IList<int> dealerIds);
        Task<IList<DealerPerformanceResultModel>> GetReportDealerPerformance(DealerPerformanceSearchModel model, IList<int> dealerIds);
        Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<int> dealerIds, DealerPerformanceReportType dealerPerformanceReportType);
        Task<int> NoOfBillingDealer(IList<int> dealerIds);
        Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands);
        Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones);
    }
}
