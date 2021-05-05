using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.interfaces
{
    public interface ICollectionPlanService
    {
        #region Collection Config
        Task<IList<CollectionConfigModel>> GetAllCollectionConfigsAsync();
        Task<CollectionConfigModel> GetCollectionConfigByIdAsync(int id);
        Task<int> UpdateCollectionConfigAsync(SaveCollectionConfigModel model);
        #endregion

        #region Collection Plan
        Task<QueryResultModel<CollectionPlanModel>> GetAllCollectionPlansAsync(QueryObjectModel query);
        Task<IList<CollectionPlanModel>> GetAllCollectionPlansAsync();
        Task<CollectionPlanModel> GetCollectionPlansByIdAsync(int id);
        Task<int> AddCollectionPlansAsync(SaveCollectionPlanModel model);
        Task<int> UpdateCollectionPlansAsync(SaveCollectionPlanModel model);
        Task<int> DeleteCollectionPlansAsync(int id);
        Task<bool> IsExitsCollectionPlansAsync(int id, int userId, string businessArea, string territory, int year = 0, int month = 0);
        #endregion
    }
}
