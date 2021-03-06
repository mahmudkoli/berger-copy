using System;
using System.Net;
using System.Threading.Tasks;
using Berger.Common.Constants;
using Berger.Odata.Services;
using BergerMsfaApi.ActiveDirectory;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.AlertNotification;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Users
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppAuthController : BaseController
    {
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly ILoginLogService _loginLogService;
        private readonly IActiveDirectoryServices _adservice;
        private readonly IAlertNotificationDataService _alertNotification;
        private readonly INotificationWorkerService _alertNotificationData;
        private readonly ITempUserLoginHistoryService _tempUserLoginHistoryService;

        public AppAuthController(
            IAuthService service, 
            IUserInfoService user, 
            ILoginLogService loginLogService, 
            IActiveDirectoryServices adservice,
            IAlertNotificationDataService alertNotification,
            INotificationWorkerService alertNotificationData,
            ITempUserLoginHistoryService tempUserLoginHistoryService)
        {
            authService = service;
            _userService = user;
            _loginLogService = loginLogService;
            _adservice = adservice;
            _alertNotification = alertNotification;
            _alertNotificationData = alertNotificationData;
            _tempUserLoginHistoryService = tempUserLoginHistoryService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticateUserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                bool loginSuccess = await _userService.IsUserNameExistAsync(model.UserName);
                if (!loginSuccess)
                {
                    ModelState.AddModelError("", "UserName or password is invalid.");
                    return ValidationResult(ModelState);
                }

                bool isAdLoginSuccess = _adservice.AuthenticateUser(model.UserName, model.Password);
                if (!isAdLoginSuccess)
                {
                    ModelState.AddModelError("", "UserName or password is invalid.");
                    return ValidationResult(ModelState);
                }

                bool isActive = await _userService.IsActiveUserAsync(model.UserName);
                if (!isActive)
                {
                    ModelState.AddModelError("", "User is not active to access. Please contact with Admin.");
                    return ValidationResult(ModelState);
                }

                //var authUser = await authService.GetJWTTokenByUserNameAsync(model.UserName);
                var authUser = await authService.GetJWTTokenByUserNameWithRefreshTokenAsync(model.UserName, HttpContext, Request, Response);

                var appVersion = HttpContext.Request?.Headers[ConstantPlatformValue.AppVersionHeaderName];

                await _tempUserLoginHistoryService.UserLoggedInLogEntryAsync(authUser.UserId, authUser.Token, true, appVersion);

                return OkResult(authUser);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpPost("activity")]
        public async Task<IActionResult> UserActivity()
        {
            try
            {
                var loginLog = await _loginLogService.UserActivityAsync();

                return OkResult(loginLog);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenModel token)
        {
            try
            {
                var response = await authService.RefreshTokenAsync(token.Token, HttpContext, Request, Response);
                return OkResult(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RefreshTokenModel token)
        {
            try
            {
                var response = await authService.RevokeTokenAsync(token.Token, HttpContext, Request);
                return OkResult(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
