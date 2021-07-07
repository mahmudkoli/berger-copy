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
    public class LoginLogService : ILoginLogService
    {
        private readonly IRepository<LoginLog> _loginLogRepo;

        public LoginLogService(IRepository<LoginLog> loginLogRepo)
        {
            _loginLogRepo = loginLogRepo;
        }

        public async Task<int> UserLoggedInLogEntryAsync(int userId, string fcmToken)
        {
            var logoutLog = await this.UserLoggedOutLogEntryAsync(userId);

            var entity = new LoginLog()
            {
                UserId = userId,
                FCMToken = fcmToken,
                IsLoggedIn = true,
                LoggedInTime = DateTime.Now,
                LoggedOutTime = DateTime.Now
            };

            var result = await _loginLogRepo.CreateAsync(entity);

            return result.Id;
        }

        public async Task<bool> UserLoggedOutLogEntryAsync(int userId)
        {
            var result = await _loginLogRepo.GetAllIncludeAsync(x => x,
                            x => x.UserId == userId && x.IsLoggedIn, 
                            x => x.OrderByDescending(o => o.LoggedInTime), 
                            null, true);

            if (result.Any())
            {
                foreach (var item in result)
                {
                    item.IsLoggedIn = false;
                }

                var returnResult = await _loginLogRepo.UpdateListAsync(result.ToList());
            }

            return true;
        }

        public async Task<IList<LoginLog>> GetAllLoggedInUsersAsync()
        {
            //var result = await _loginLogRepo.GetAllIncludeAsync(
            //                       x => x,
            //                       x => x.IsLoggedIn,
            //                       null,
            //                       null,
            //                       true
            //                   );

            //return result;
            return new List<LoginLog>();
        }

        public async Task<bool> UserActivityAsync(int? userIdp = null, string fcmToken = null)
        {
            var userId = userIdp ?? AppIdentity.AppUser.UserId;
            var currentDate = DateTime.Now;

            var result = new LoginLog
            {
                UserId = userId,
                FCMToken = fcmToken,
                IsLoggedIn = true,
                LoggedInTime = currentDate,
                LoggedOutTime = currentDate
            };

            await _loginLogRepo.CreateAsync(result);

            return true;
        }
    }
}
