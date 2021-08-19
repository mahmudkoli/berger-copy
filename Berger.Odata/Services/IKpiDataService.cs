using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IKpiDataService
    {
        Task<List<TerritoryTargetAchievementResultModel>> GetTerritoryTargetAchivement(SalesTargetAchievementSearchModel model);
        Task<List<AppTargetAchievementResultModel>> GetAppSalesTargetAchievement(SalesTargetAchievementSearchModel model);
        Task<List<DealerWiseTargetAchievementResultModel>> GetDealerWiseTargetAchivement(DealerWiseTargetAchievementSearchModel model);
        Task<List<AppTargetAchievementResultModel>> GetAppDealerWiseTargetAchievement(DealerWiseTargetAchievementSearchModel model);
        Task<List<ProductWiseTargetAchievementResultModel>> GetProductWiseTargetAchivement(ProductWiseTargetAchievementSearchModel model);
        Task<List<AppProductWiseTargetAchievementResultModel>> GetAppProductWiseTargetAchievement(ProductWiseTargetAchievementSearchModel model);
    }
}
