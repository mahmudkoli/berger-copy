using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerOpeningController : BaseController
    {
        private ILogger<AppDealerOpeningController> _logger;
        private readonly IDealerOpeningService _dealerOpeningSvc;

        public AppDealerOpeningController(
            ILogger<AppDealerOpeningController> logger
            , IDealerOpeningService dealerOpeningSvc)
        {
            _logger = logger;
            _dealerOpeningSvc = dealerOpeningSvc;
        }

        [HttpGet("GetDealerOpeningList")]
        public async Task<IActionResult> GetDealerOpeningListAsync()
        {
            try
            {
                var result = await _dealerOpeningSvc.AppGetDealerOpeningListByCurrentUserAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateDealerOpening")]
        //[RequestSizeLimit(31457280)] // 30MB
        //[DisableRequestSizeLimit]
        public async Task<IActionResult> CreateDealerOpeningAsync([FromBody] DealerOpeningModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _dealerOpeningSvc.AppCreateDealerOpeningAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
