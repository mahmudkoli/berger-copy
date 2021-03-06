using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Setup;
using X.PagedList;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IDropdownService
    {
        Task<IEnumerable<PainterCompanyMTDValueModel>> GetCompanyList(int PainterCallId);
        Task<IEnumerable<DropdownModel>> GetDropdownList();
        Task<IPagedList<DropdownModel>> GetDropdownListPaging(int index,int pageSize);
        Task<DropdownModel> GetDropdownById(int id);
        Task<IEnumerable<DropdownModel>> GetDropdownByTypeCd(string typeCode);
        Task<IEnumerable<DropdownModel>> GetDropdownByTypeCd(IList<string> typeCodes);
        Task<IEnumerable<DropdownModel>> GetDropdownByTypeId(int typeId);
        Task<int> GetLastSquence(int id,int typeId);
        Task<IEnumerable<DropdownType>> GetDropdownTypeList();
     
        Task<DropdownModel> CreateAsync(DropdownModel model);
        Task<DropdownModel> UpdateAsync(DropdownModel model);
        Task<int> DeleteAsync(int id);


        Task<bool> IsExistAsync(int id);
      
      
     
    }
}
