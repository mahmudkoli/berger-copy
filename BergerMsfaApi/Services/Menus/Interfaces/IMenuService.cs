using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Menus;
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetMenusAsync();
        Task<IEnumerable<MenuModel>> GetActiveMenusAsync();
        Task<IEnumerable<MenuModel>> GetChildMenusAsync();
        Task<IPagedList<MenuModel>> GetPagedMenusAsync(int pageNumber, int pageSize);
        Task<MenuModel> GetMenuAsync(int id);
        Task<MenuModel> SaveAsync(MenuModel model);
        Task<MenuModel> CreateAsync(MenuModel model);
        Task<MenuModel> CreateAndUpdateParentAsync(MenuModel model);
        Task<MenuModel> UpdateAsync(MenuModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsMenuExistAsync(string menuName, int id, TypeEnum typeEnum, int parentId);
        Task<List<MenuPermissionModel>> AssignRoleToMenuAsync(List<MenuPermissionModel> model, int roleId);
        Task<IEnumerable<MenuModel>> GetPermissionMenus(int roleId);
        Task<IEnumerable<MenuModel>> GetMenusAsync(int type);
        Task<List<MenuPermissionModel>> AssignEmpToMenuAsync(List<MenuPermissionModel> model, int empRleId, int type);
        Task<IEnumerable<MobileAppMenuPermissionModel>> GetPermissionMenusByEmpRoleId(int roleId);

        Task<List<MenuModel>> GetAlertPermissionsByEmp(EnumEmployeeRole employeeRole, TypeEnum typeEnum);
    }
}
