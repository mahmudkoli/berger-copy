using System;
using System.Threading.Tasks;
using BergerMsfaApi.ActiveDirectory;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Users
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly IActiveDirectoryServices _adservice;

        public AuthController(
            IAuthService service, 
            IUserInfoService user, 
            IActiveDirectoryServices adservice)
        {
            authService = service;
            _userService = user;
            _adservice = adservice;
        }

        [AllowAnonymous]
        [HttpPost("login")]
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

                var authUser = await authService.GetJWTTokenByUserNameAsync(model.UserName);

                return OkResult(authUser);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
