using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Scheme.interfaces
{
    public interface ISchemeService
    {
        #region Scheme Master
        Task<IPagedList<SchemeMasterModel>> GetAllSchemeMastersAsync(int index, int pageSize, string search);
        Task<QueryResultModel<SchemeMasterModel>> GetAllSchemeMastersAsync(QueryObjectModel query);
        Task<IList<SchemeMasterModel>> GetAllSchemeMastersAsync();
        Task<SchemeMasterModel> GetSchemeMasterByIdAsync(int id);
        Task<int> AddSchemeMasterAsync(SaveSchemeMasterModel model);
        Task<int> UpdateSchemeMasterAsync(SaveSchemeMasterModel model);
        Task<int> DeleteSchemeMasterAsync(int id); 
        Task<object> GetAllSchemeMastersForSelectAsync();
        #endregion

        #region Scheme Details
        Task<IPagedList<SchemeDetailModel>> GetAllSchemeDetailsAsync(int index, int pageSize, string search);
        Task<QueryResultModel<SchemeDetailModel>> GetAllSchemeDetailsAsync(QueryObjectModel query);
        Task<IList<SchemeDetailModel>> GetAllSchemeDetailsAsync();
        Task<SchemeDetailModel> GetSchemeDetailsByIdAsync(int id);
        Task<int> AddSchemeDeatilsAsync(SaveSchemeDetailModel model);
        Task<int> UpdateSchemeDetailsAsync(SaveSchemeDetailModel model);
        Task<int> DeleteSchemeDetailsAsync(int id);
        #endregion
    }
}
