using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Scheme.interfaces
{
    public interface ISchemeService
    {
        Task<IPagedList<SchemeMasterModel>> PortalGetSchemeMasters(int index, int pageSize, string search);
        Task<IEnumerable<SchemeMasterModel>> AppGetSchemeMasters();

        Task<SchemeMasterModel> PortalGetSchemeMastersById(int Id );
        Task<SchemeMasterModel> PortalCreateSchemeMasters(SchemeMasterModel model);
        Task<SchemeMasterModel> PortalUpdateSchemeMasters(SchemeMasterModel model);
        Task<int> PortalDeleteSchemeMasters(int Id);

        Task<IPagedList<SchemeDetailModel>> PortalGetcShemeDetailWithMaster(int index, int pageSize, string search);
        Task<IEnumerable<SchemeDetailModel>> AppGetcShemeDetailWithMaster();

        Task<IEnumerable<SchemeDetailModel>> PortalGetSchemeDelails();
        Task<SchemeDetailModel> PortalGetSchemeDetailById(int Id);
        Task<SchemeDetailModel> PortalCreateSchemeDeatil(SchemeDetailModel model);
        Task<SchemeDetailModel> PortalUpdateSchemeDetail(SchemeDetailModel model);
        Task<int> PortalDeleteSchemeDetail(int Id);
        Task<bool> IsSchemeMasterAlreadyExist(int Id);
        Task<bool> IsSchemeDetailAlreadyExist(int Id);

        //   Task<IEnumerable<SchemeMasterModel>> PortalGetSchemeMasterWithDetail();
    }
}
