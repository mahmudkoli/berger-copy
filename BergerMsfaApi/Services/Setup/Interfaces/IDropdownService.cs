using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.Setup;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IDropdownService
    {
      
        Task<IEnumerable<DropdownModel>> GetDropdownList();
        Task<DropdownModel> GetDropdownById(int id);
        Task<IEnumerable<DropdownModel>> GetDropdownByTypeCd(string typeCode);
        Task<IEnumerable<DropdownModel>> GetDropdownByTypeId(int typeId);
        Task<int> GetLastSquence(int id,int typeId);
        Task<IEnumerable<DropdownType>> GetDropdownTypeList();
     
        Task<DropdownModel> CreateAsync(DropdownModel model);
        Task<DropdownModel> UpdateAsync(DropdownModel model);
        Task<int> DeleteAsync(int id);


        Task<bool> IsExistAsync(int id);
      
      
     
    }
}
