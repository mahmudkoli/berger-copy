using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class TempUserLoginHistoryService : ITempUserLoginHistoryService
    {
        private readonly IRepository<TempUserLoginHistory> _loginRepo;

        public TempUserLoginHistoryService(IRepository<TempUserLoginHistory> loginRepo)
        {
            _loginRepo = loginRepo;
        }

        public async Task<int> UserLoggedInLogEntryAsync(int userId, string jwtToken, bool fromAppLogin)
        {
            var entity = new TempUserLoginHistory()
            {
                UserId = userId,
                JwtToken = jwtToken,
                FromAppLogin = fromAppLogin,
                LoggedInTime = DateTime.Now
            };

            var result = await _loginRepo.CreateAsync(entity);

            return result.Id;
        }
    }
}
