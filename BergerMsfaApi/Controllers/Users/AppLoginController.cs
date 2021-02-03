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
    public class AppLoginController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly IActiveDirectoryServices _adservice;



        public AppLoginController(IConfiguration config, IAuthService service, 
            IUserInfoService user, IActiveDirectoryServices adservice)
        {
            _config = config;
            authService = service;
            _userService = user;
            _adservice = adservice;
        }


        private String username => $"nizamuddinbs"; // fazlur1
        private String password => $"5~nEVER~cATCH:";

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //temporary
            //model.UserName = username;
            //model.Password = password;

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
                        //Check db for user
                       loginSuccess = await _userService.IsUserNameExistAsync(model.UserName);
                    //}
                    //else
                    //{
                    //    return Unauthorized();
                    //}

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
    }
}
