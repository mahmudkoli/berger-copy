using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using BergerMsfaApi.Models.Somporko.Users;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Somporko.Users.Interfaces
{
    public interface ISomporkoAuthService
    {
        Task<bool> IsUserExistAsync(string username, string password);
        Task<bool> IsActiveUserAsync(string username);
        Task<SomporkoAuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName);
    }
}
