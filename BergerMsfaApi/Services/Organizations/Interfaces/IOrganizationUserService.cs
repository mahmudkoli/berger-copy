using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Organizations;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Organizations.Interfaces
{
    public interface IOrganizationUserService
    {
        Task<IEnumerable<UserInfoModel>> GetUsersAsync();
        Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync();
        Task<UserInfoModel> GetUserAsync(int id);
        Task<UserInfoModel> SaveAsync(UserInfoModel model);
        Task<OrganizationRoleMappingModel> SaveOrganizationRoleLinkWithUserAsync(OrganizationRoleMappingModel model);
        Task<UserInfoModel> CreateAsync(UserInfoModel model);
        Task<OrganizationRoleMappingModel> CreateOrganizationRoleLinkWithUserAsync(OrganizationRoleMappingModel model);
        Task<UserInfoModel> UpdateAsync(UserInfoModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsUserExistAsync(string code, int id);
        Task<bool> IsOrganizationRoleLinkWithUserExistAsync(int organizationRoleId, int userInfoId);
    }
}
