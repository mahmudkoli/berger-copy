﻿using System;
using System.Net;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Somporko.Users;
using BergerMsfaApi.Services.Somporko.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Somporko.Users
{
    [Somporko]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SomporkoAppAuthController : BaseController
    {
        private readonly ISomporkoAuthService _authService;

        public SomporkoAppAuthController(
            ISomporkoAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(SomporkoAuthenticateUserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] SomporkoLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                //var pass = Berger.Common.Extensions.SecurityExtension.ToEncryptString("msfa@$omp0rko@pp@2021", "Auf2eZehjfg6tfgttlgs3hfuRrf8fday");

                bool loginSuccess = await _authService.IsUserExistAsync(model.UserName, model.Password);
                if (!loginSuccess)
                {
                    ModelState.AddModelError("", "UserName or password is invalid.");
                    return ValidationResult(ModelState);
                }

                bool isActive = await _authService.IsActiveUserAsync(model.UserName);
                if (!isActive)
                {
                    ModelState.AddModelError("", "User is not active to access. Please contact with Admin.");
                    return ValidationResult(ModelState);
                }

                var authUser = await _authService.GetJWTTokenByUserNameAsync(model.UserName);

                return OkResult(authUser);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
