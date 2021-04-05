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
        Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(QuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(QuarterlyPerformanceSearchModel model);
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
