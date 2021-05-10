using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface ILoginLogService
    {
        Task<int> UserLoggedInLogEntryAsync(int userId, string fcmToken);
        Task<bool> UserLoggedOutLogEntryAsync(int userId);
        Task<IList<LoginLog>> GetAllLoggedInUsersAsync();
        Task<bool> UserActivityAsync(int? userIdp = null, string fcmToken = null);
    }
}