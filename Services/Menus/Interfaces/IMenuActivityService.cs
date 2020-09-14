using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Menus;
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Interfaces
{
    public interface IMenuActivityService
    {
        Task<IEnumerable<MenuActivityModel>> GetAllMenusActivityAsync();
        Task<IPagedList<MenuActivityModel>> GetPagedMenusAsync(int pageNumber, int pageSize);
        Task<MenuActivityModel> GetMenuActivityAsync(int id);
        Task<MenuActivityModel> SaveAsync(MenuActivityModel model);
        Task<MenuActivityModel> CreateAsync(MenuActivityModel model);
        Task<MenuActivityModel> CreateAndUpdateParentAsync(MenuActivityModel model);
        Task<MenuActivityModel> UpdateAsync(MenuActivityModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsMenuActivityExistAsync(string name, string code, int id);

        Task<IEnumerable<MenuActivityModel>> GetAllMenuActivityById(int id);

        Task<IEnumerable<MenuActivityPermissionVm>> GetAllMenuActivityPermissionByRoleId(int id); /////to get all menu id under any roleid
    }
}
