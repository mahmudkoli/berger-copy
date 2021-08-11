using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.interfaces
{
    public interface IUniverseReachAnalysisService
    {
        #region Collection Plan
        Task<QueryResultModel<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissAsync(UniverseReachAnalysisQueryObjectModel query);
        Task<QueryResultModel<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissByCurrentUserAsync(UniverseReachAnalysisQueryObjectModel query);
        Task<IList<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissAsync();
        Task<UniverseReachAnalysisModel> GetUniverseReachAnalysissByIdAsync(int id);
        Task<int> AddUniverseReachAnalysissAsync(SaveUniverseReachAnalysisModel model);
        Task<int> UpdateUniverseReachAnalysissAsync(SaveUniverseReachAnalysisModel model);
        Task<int> DeleteUniverseReachAnalysissAsync(int id);
        Task<bool> IsExitsUniverseReachAnalysissAsync(int id, string businessArea, string territory, string fiscalYear = null);
        Task<SaveAppUniverseReachAnalysisModel> GetAppUniverseReachAnalysissAsync(AppUniverseReachAnalysisQueryObjectModel query);
        Task<int> UpdateAppUniverseReachAnalysissAsync(SaveAppUniverseReachAnalysisModel model);
        string GetCurrentFiscalYear();
        #endregion
    }
}
