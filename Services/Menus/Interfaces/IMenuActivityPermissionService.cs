using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Menus;
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Interfaces
{
    public interface IMenuActivityPermissionService
    {
        Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionAsync();
        Task<IPagedList<MenuActivityPermissionModel>> GetPagedMenuPermissionAsync(int pageNumber, int pageSize);
        Task<MenuActivityPermissionModel> GetMenuActivityPermissionAsync(int id);
        Task<MenuActivityPermissionModel> SaveAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> CreateAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> CreateAndUpdateParentAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> UpdateAsync(MenuActivityPermissionModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsMenuActivityPermissionExistAsync(int roleId, int id);
        Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionByRoleIdAsync(int id);
        Task<List<MenuActivityPermissionModel>> CreateOrUpdateAllAsync(List<MenuActivityPermissionVm> modelList);
        Task<List<ActivityPermissionModel>> GetActivityPermissions(int roleId);
    }
}
