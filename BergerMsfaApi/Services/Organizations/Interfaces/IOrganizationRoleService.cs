using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Organizations;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Organizations.Interfaces
{
    public interface IOrganizationRoleService
    {
       
        Task<IPagedList<OrganizationRoleModel>> GetPagedOrganizationRolesAsync(int pageNumber, int pageSize);
      
        Task<OrganizationRoleModel> GetOrganizationRoleAsync(int id);
        Task<OrganizationRoleModel> SaveAsync(OrganizationRoleModel model);
  
        Task<OrganizationRoleModel> UpdateAsync(OrganizationRoleModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsOrganizationRoleExistAsync(string code, int id);
   
        Task<UserRoleMappingModel> SaveOrganizationRoleLinkWithUserAsync(UserRoleMappingModel model);
        Task<bool> IsOrganizationRoleLinkWithUserExistAsync(int roleId, int userInfoId);
        Task<IEnumerable<OrganizationRoleModel>> GetOrganizationUserRoleAsync();
        Task<List<int>> GetOrgenizationUserByRole(int orgRole);
    }
}
