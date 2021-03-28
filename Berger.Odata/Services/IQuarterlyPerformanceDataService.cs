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
        #endregion

        #region Portal Report
        Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(PortalQuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(PortalQuarterlyPerformanceSearchModel model);
        #endregion
    }
}
