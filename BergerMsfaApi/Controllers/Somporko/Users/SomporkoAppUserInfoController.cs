using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Somporko.Users
{
    [Somporko]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SomporkoAppUserInfoController : BaseController
    {
        private readonly IUserInfoService _userService;

        public SomporkoAppUserInfoController(
            IUserInfoService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _userService.GetUsersAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
