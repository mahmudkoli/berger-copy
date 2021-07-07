using System;
using System.Threading.Tasks;
using BergerMsfaApi.ActiveDirectory;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Users
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class UserInfoController : BaseController
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IUserInfoService _userInfoService;
        private readonly IActiveDirectoryServices _adService;

        public UserInfoController(
            IUserInfoService userService, 
            ILogger<UserInfoController> logger, 
            IActiveDirectoryServices adService)
        {
            this._logger = logger;
            this._userInfoService = userService;
            this._adService = adService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _userInfoService.GetUsersAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("paged-users")]
        public async Task<IActionResult> GetPagedUsers()
        {
            try
            {
                var result = await _userInfoService.GetPagedUsersAsync(1, 20);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _userInfoService.GetUserAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("getaduser/{username}")]
        public async Task<IActionResult> GetAdUser(string username)
        {
            try
            {
                var result = _adService.GetUserByUserName(username);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] SaveUserInfoModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                var result = await _userInfoService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] SaveUserInfoModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationResult(ModelState);

                var result = await _userInfoService.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userInfoService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpPost("rolelinkwithuser")]
        public async Task<IActionResult> RoleLinkWithUser([FromBody]UserRoleMappingModel model)
        {
            try
            {
                var isExist = await _userInfoService.IsRoleLinkWithUserExistAsync(model.RoleId, model.UserInfoId);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.RoleId), "This User has already in this role");
                    return ValidationResult(ModelState);
                }

                var result = await _userInfoService.SaveRoleLinkWithUserAsync(model);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
