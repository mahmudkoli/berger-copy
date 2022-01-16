using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IQuarterlyPerformanceDataService
    {
        #region App Report
        Task<IList<AppQuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model);
        Task<IList<AppQuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<AppQuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<AppQuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<AppQuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(QuarterlyPerformanceSearchModel model);
        #endregion

        #region Portal Report
        Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(PortalQuarterlyPerformanceSearchModel model);
        #endregion
    }
}
