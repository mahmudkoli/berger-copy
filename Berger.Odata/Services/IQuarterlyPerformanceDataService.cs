using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IQuarterlyPerformanceDataService
    {
        Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model);
        Task<IList<QuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(OdataQuartPerformSearchModel model);

        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(OdataQuartPerformSearchModel model);

        Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(OdataQuartPerformSearchModel model);
    }
}
