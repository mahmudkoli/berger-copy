using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IMTSDataService
    {
        Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model);
        Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model);
        Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModel model);

        Task<IList<MTSDataModel>> GetMyTargetMts(DateTime date, IList<int> dealerIds, string division,
            MyTargetReportType targetReportType, EnumVolumeOrValue volumeOrValue);
    }
}
