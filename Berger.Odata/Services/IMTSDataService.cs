using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Model;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IMTSDataService
    {
        Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model);
        Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model);
        Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModel model);

        Task<IList<MTSDataModel>> GetMyTargetMts(DateTime date, IList<string> dealerIds, string division,
            MyTargetReportType targetReportType, EnumVolumeOrValue volumeOrValue, EnumMyTargetBrandType brandType);
        Task<IList<MTSDataModel>> GetMTDTarget(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategoryType? category, EnumMyTargetBrandType? type);
    }
}
