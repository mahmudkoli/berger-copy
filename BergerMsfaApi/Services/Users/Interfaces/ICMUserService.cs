using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface ICMUserService
    {
        Task<bool> LoginCMUser(LoginModel model);
        Task<UserViewModel> GetCMUserByLogin(LoginModel model);
        Task<IEnumerable<UserViewModel>> GetAllUserAsync();
        Task<UserViewModel> GetUserAsync(int id);
        Task<UserViewModel> SaveAsync(UserViewModel model);

        Task<UserViewModel> CreateUserAsync(UserViewModel model);

        Task<UserViewModel> UpdateAsync(UserViewModel model);
        Task<int> DeleteAsync(int id);

        Task<bool> IsUserExistAsync(string email, int id);
        Task<IEnumerable<UserViewModel>> GetCMUserByFMIdAsync(int id);
        Task<(IEnumerable<UserViewModel> Data, string Message)> ExcelSaveToDatabaseAsync(DataTable datatable);
        Task<IEnumerable<UserViewModel>> GetAllCMUsersByCurrentUserIdAsync();
    }
}

