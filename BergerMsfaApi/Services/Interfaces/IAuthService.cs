using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> GetJWTTokenByUserNameAsync(string userName);
        Task<object> GetJWTToken(PortalLoginModel model);

    }
}
