using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Logs;
using BergerMsfaApi.Services.Logs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Logs
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ApplicationLogController : BaseController
    {
        private readonly IApplicationLogService _mobileAppLogService;

        public ApplicationLogController(
            IApplicationLogService mobileAppLogService)
        {
            _mobileAppLogService = mobileAppLogService;
        }

        [AllowAnonymous]
        [HttpPost("CreateAppLog")]
        public async Task<IActionResult> CreateDealerSalesCall([FromBody] MobileAppLogModel model)
        {
            try
            {
                var result = await _mobileAppLogService.AddMobileAppLogAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
