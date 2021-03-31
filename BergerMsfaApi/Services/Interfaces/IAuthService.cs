using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName);
        Task<IList<int>> GetDealerByUserId(int userId);
    }
}
