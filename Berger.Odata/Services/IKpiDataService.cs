using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IKpiDataService
    {
        Task<List<TerritoryTargetAchievementResultModel>> GetTerritoryTargetAchivement(TerritoryTargetAchievementSearchModel model);
        Task<List<AppTargetAchievementResultModel>> GetAppTargetAchievement(TerritoryTargetAchievementSearchModel model);
        Task<List<DealerWiseTargetAchievementResultModel>> GetDealerWiseTargetAchivement(DealerWiseTargetAchievementSearchModel model);
        Task<List<ProductWiseTargetAchievementResultModel>> GetProductWiseTargetAchivement(ProductWiseTargetAchievementSearchModel model);
    }
}
