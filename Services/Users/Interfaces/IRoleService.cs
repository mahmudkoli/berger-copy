using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> GetRolesAsync();
        Task<IPagedList<RoleModel>> GetPagedRolesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<RoleModel>> GetQueryRolesAsync();
        Task<RoleModel> GetRoleAsync(int id);
        Task<RoleModel> SaveAsync(RoleModel model);
        Task<RoleModel> CreateAsync(RoleModel model);
        Task<RoleModel> UpdateAsync(RoleModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsRoleExistAsync(string name, int id);
        Task<bool> IsRoleNameExistAsync(string name);
    }
}