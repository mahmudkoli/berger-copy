using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.ActiveDirectory;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Core;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BergerMsfaApi.Controllers.Users
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppAuthController : BaseController
    {
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly ILoginLogService _loginLogService;
        private readonly IActiveDirectoryServices _adservice;

        public AppAuthController(
            IAuthService service, 
            IUserInfoService user, 
            ILoginLogService loginLogService, 
            IActiveDirectoryServices adservice)
        {
            authService = service;
            _userService = user;
            this._loginLogService = loginLogService;
            _adservice = adservice;
        }

        private string username => $"nizamuddinbs"; // fazlur1
        //private string password => $"5~nEVER~cATCH:";
        private string password => $"**33!wave!GAVE!70**";

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO: need to comment out
            //model.UserName = username;
            //model.Password = password;

            try
            {
                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                //TODO: need to uncomment 
                //bool isAdLoginSuccess = _adservice.AuthenticateUser(model.UserName, model.Password);
                //if (!isAdLoginSuccess)
                //{
                //    ModelState.AddModelError("", "UserName or password is invalid.");
                //    return ValidationResult(ModelState);
                //}

                bool loginSuccess = await _userService.IsUserNameExistAsync(model.UserName);
                if (!loginSuccess)
                {
                    ModelState.AddModelError("", "UserName or password is invalid.");
                    return ValidationResult(ModelState);
                }

                var authUser = await authService.GetJWTTokenByUserNameAsync(model.UserName);

                #region Login Log
                //if(!string.IsNullOrWhiteSpace(model.FCMToken))
                //{
                var loginLogId = await _loginLogService.UserLoggedInLogEntryAsync(authUser.UserId, model.FCMToken);
                //}
                #endregion

                return OkResult(authUser);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [Authorize]
        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> Logout(int userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                var loginLog = await _loginLogService.UserLoggedOutLogEntryAsync(userId);

                return OkResult(loginLog);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
