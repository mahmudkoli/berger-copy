using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> GetJWTToken(LoginModel model);
        Task<object> GetJWTToken(PortalLoginModel model);

    }
}
