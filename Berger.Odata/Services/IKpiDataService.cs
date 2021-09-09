using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPReports;

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

        Task<IList<KPIPerformanceReport>> GetKpiPerformanceReport(Expression<Func<KPIPerformanceReport,
                KPIPerformanceReport>> selectProperty,
            string startDate, string endDate, List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, List<string> brands = null,
            string customerNo = null,
            string division = null
        );
    }
}
