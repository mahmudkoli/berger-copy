using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IKpiDataService
    {
        Task<TerritoryTargetAchievementResultModel> GetTerritoryTargetAchivement(TerritoryTargetAchievementSearchModel model);
        Task<DealerWiseTargetAchievementResultModel> GetDealerWiseTargetAchivement(DealerWiseTargetAchievementSearchModel model);
    }
}
