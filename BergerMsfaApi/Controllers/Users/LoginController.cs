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
    public class LoginController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly ICMUserService _cmuserservice;
        private readonly IActiveDirectoryServices _adservice;



        public LoginController(IConfiguration config, IAuthService service, 
            IUserInfoService user, ICMUserService userService, IActiveDirectoryServices adservice)
        {
            _config = config;
            authService = service;
            _userService = user;
            _cmuserservice = userService;
            _adservice = adservice;
        }


        private String username => $"nizamuddinbs"; // fazlur1
        private String password => $"5~nEVER~cATCH:";

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //temporary
            //model.UserName = username;

            model.Password = password;

            var apiResult = new ApiResponse<IEnumerable<LoginModel>>
            {
                Data = new List<LoginModel>()
            };

            if (ModelState.IsValid)
            {
                try
                {
                    //bool isAdLoginSuccess =_adservice.AuthenticateUser(model.UserName, model.Password);
                     bool loginSuccess = false;

                    //if (isAdLoginSuccess)
                    //{
                    //    //Check db for user
                    //   loginSuccess = await _userService.IsUserNameExistAsync(model.UserName);
                    //}
                    //else
                    //{
                    //    return Unauthorized();
                    //}
                     loginSuccess = await _userService.IsUserNameExistAsync(model.UserName); 

                    if (loginSuccess)
                    {
                        var result = await authService.GetJWTTokenByUserNameAsync(model.UserName);
                        return OkResult(result);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    ex.ToWriteLog();

                    apiResult.StatusCode = 401;
                    apiResult.Status = "Fail";
                    apiResult.Msg = ex.Message;

                    return Unauthorized(apiResult);
                }
            }

            return BadRequest();
        }

        [HttpPost("portallogin")]
        public async Task<IActionResult> AdUserLogin([FromBody] PortalLoginModel model)
        {
            var apiResult = new ApiResponse<IEnumerable<PortalLoginModel>>
            {
                Data = new List<PortalLoginModel>()
            };
            if (ModelState.IsValid)
            {
                try
                {
                    bool IsAdUserAvailable = await _userService.IsUserExistAsync(model.UserName);
                    if (IsAdUserAvailable)
                    {
                        var result = authService.GetJWTToken(model);
                        return OkResult(result);
                    }
                    else
                    {
                        return BadRequest();
                    }

                }
                catch (Exception ex)
                {
                    ex.ToWriteLog();

                    apiResult.StatusCode = 500;
                    apiResult.Status = "Fail";
                    apiResult.Msg = ex.Message;
                    return BadRequest(apiResult);
                }
            }

            return BadRequest();
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getuser")]
        public IActionResult GetUser()
        {
            var apiResult = new ApiResponse<IEnumerable<UserInfo>>
            {
                Data = new List<UserInfo>()
            };
            try
            {
                UserInfo userInfo = new UserInfo();
                IEnumerable<object> items = User.Claims;
                //foreach (var item in items)
                //{
                //    userInfo.Name = AppIdentity.AppUser;
                //    //userInfo.Roles = item.
                //    return OkResult(userInfo);

                //}

                return OkResult(AppIdentity.AppUser);

            }
            catch (Exception ex)
            {

                ex.ToWriteLog();

                apiResult.StatusCode = 500;
                apiResult.Status = "Fail";
                apiResult.Msg = ex.Message;
                return BadRequest(apiResult);
            }



        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getaduser")]
        public IActionResult GetAdUser()
        {
            var apiResult = new ApiResponse<IEnumerable<UserInfo>>
            {
                Data = new List<UserInfo>()
            };
            try
            {
                UserInfoModel userInfo = new UserInfoModel()
                {
                    Name = AppIdentity.AppUser.FullName,
                    RoleId = AppIdentity.AppUser.ActiveRoleId,
                    RoleIds = AppIdentity.AppUser.RoleIdList,
                    NodeId = AppIdentity.AppUser.NodeId,
                    PhoneNumber = AppIdentity.AppUser.Phone,
                    EmployeeId = AppIdentity.AppUser.EmployeeId,
                    Email = AppIdentity.AppUser.Email,
                    RoleName = AppIdentity.AppUser.ActiveRoleName

                };


                return OkResult(userInfo);

            }
            catch (Exception ex)
            {

                ex.ToWriteLog();

                apiResult.StatusCode = 500;
                apiResult.Status = "Fail";
                apiResult.Msg = ex.Message;
                return BadRequest(apiResult);
            }
        }
    }
}
