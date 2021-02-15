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
        Task<IList<ValueTargetResultModel>> GetValueTarget(MTSSearchModel model);
    }
}
