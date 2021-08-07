using BergerMsfaApi.Models.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Report.Interfaces
{
    public interface IKPIReportService
    {
        Task<IList<StrikeRateKPIReportResultModel>> PremiumBrandBillingStrikeRateKPIReportAsync(StrikeRateKPIReportSearchModel query, EnumReportFor reportFor);
        Task<IList<BusinessCallBaseKPIReportResultModel>> GetBusinessCallKPIReportAsync(BusinessCallKPIReportSearchModel query, EnumReportFor reportFor);
        Task<IList<BillingAnalysisKPIReportResultModel>> GetBillingAnalysisKPIReportAsync(BillingAnalysisKPIReportSearchModel query);
        Task<IList<CollectionPlanKPIReportResultModel>> GetFinancialCollectionPlanKPIReportAsync(CollectionPlanKPIReportSearchModel query);
    }
}
