using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Scheme.interfaces
{
    public interface ISchemeService
    {
        Task<IEnumerable<SchemeMasterModel>> PortalGetSchemeMasters( );
   
        Task<SchemeMasterModel> PortalGetSchemeMastersById(int Id );
        Task<SchemeMasterModel> PortalCreateSchemeMasters(SchemeMasterModel model);
        Task<SchemeMasterModel> PortalUpdateSchemeMasters(SchemeMasterModel model);
        Task<int> PortalDeleteSchemeMasters(int Id);

        Task<IEnumerable<SchemeDetailModel>> PortalGetcShemeDetailWithMaster();

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
