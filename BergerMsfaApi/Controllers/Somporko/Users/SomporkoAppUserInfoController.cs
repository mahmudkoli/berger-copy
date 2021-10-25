using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Somporko.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Somporko.Users
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SomporkoAppUserInfoController : BaseController
    {
        private readonly ISomporkoUserInfoService _userService;

        public SomporkoAppUserInfoController(
            ISomporkoUserInfoService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsersAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
