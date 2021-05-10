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
            //var loginLog = await this.UserLoggedOutLogEntryAsync(userId);

            //var entity = new LoginLog()
            //{
            //    UserId = userId,
            //    FCMToken = fcmToken,
            //    IsLoggedIn = true,
            //    LoggedInTime = DateTime.Now
            //};

            //var result = await _loginLogRepo.CreateAsync(entity);
            await this.UserActivityAsync(userId, fcmToken);

            return 1;
        }

        public async Task<bool> UserLoggedOutLogEntryAsync(int userId)
        {
            var result = await _loginLogRepo.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.UserId == userId && x.IsLoggedIn, 
                            x => x.OrderByDescending(o => o.LoggedInTime), 
                            null, true);

            if (result != null)
            {
                result.IsLoggedIn = false;
                result.LoggedOutTime = DateTime.Now;

                await _loginLogRepo.UpdateAsync(result);
            }

            return true;
        }

        public async Task<IList<LoginLog>> GetAllLoggedInUsersAsync()
        {
            var result = await _loginLogRepo.GetAllIncludeAsync(
                                   x => x,
                                   x => x.IsLoggedIn,
                                   null,
                                   null,
                                   true
                               );

            return result;
        }

        public async Task<bool> UserActivityAsync(int? userIdp = null, string fcmToken = null)
        {
            var userId = userIdp ?? AppIdentity.AppUser.UserId;
            var currentDate = DateTime.Now;

            var result = await _loginLogRepo.GetFirstOrDefaultIncludeAsync(x => x,
                                x => x.UserId == userId && x.LoggedInTime.Date == currentDate.Date,
                                null, null, true);

            if (result == null)
            {
                result = new LoginLog
                {
                    UserId = userId,
                    FCMToken = fcmToken,
                    IsLoggedIn = true,
                    LoggedInTime = currentDate,
                    LoggedOutTime = currentDate
                };

                await _loginLogRepo.CreateAsync(result);
            }
            else 
            {
                result.FCMToken = fcmToken ?? result.FCMToken;
                result.LoggedOutTime = DateTime.Now;

                await _loginLogRepo.UpdateAsync(result);
            }

            #region previous data logged out
            var results = await _loginLogRepo.GetAllIncludeAsync(x => x,
                                x => x.UserId == userId && (x.LoggedOutTime??x.LoggedInTime).Date < currentDate.Date,
                                null, null, true);

            if (results.Any())
            {
                foreach (var item in results)
                {
                    item.IsLoggedIn = false;
                }
                await _loginLogRepo.UpdateListAsync(results.ToList());
            }
            #endregion

            return true;
        }
    }
}
