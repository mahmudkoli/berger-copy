using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IMTSDataService
    {
        Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model);
        Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model);
        Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModelBase model);
        Task<IList<MTSDataModel>> GetMTDTarget(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type);
    }
}
