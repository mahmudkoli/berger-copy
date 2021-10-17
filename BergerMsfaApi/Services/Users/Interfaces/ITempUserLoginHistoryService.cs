using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface ITempUserLoginHistoryService
    {
        Task<int> UserLoggedInLogEntryAsync(int userId, string jwtToken, bool fromAppLogin);
    }
}