using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName);
        Task<IList<string>> GetDealerByUserId(int userId);
        AreaSearchCommonModel GetLoggedInUserArea();
    }
}
