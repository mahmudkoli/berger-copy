using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using BergerMsfaApi.Models.Users;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName);
        Task<AuthenticateUserModel> GetJWTTokenByUserNameWithRefreshTokenAsync(string userName, HttpContext context, HttpRequest request, HttpResponse response);
        Task<IList<string>> GetDealerByUserId(int userId);
        AreaSearchCommonModel GetLoggedInUserArea();
        Task<AuthenticateUserModel> RefreshTokenAsync(HttpContext context, HttpRequest request, HttpResponse response);
        Task<bool> RevokeTokenAsync(string token, HttpContext context, HttpRequest request);
    }
}
