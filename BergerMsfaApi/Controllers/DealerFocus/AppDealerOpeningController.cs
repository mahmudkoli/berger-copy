using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerOpeningController : BaseController
    {
        private ILogger<AppDealerOpeningController> _logger;
        private readonly IDealerOpeningService _dealerOpeningSvc;
        public AppDealerOpeningController(ILogger<AppDealerOpeningController> logger
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
                var result = await _dealerOpeningSvc.AppGetDealerOpeningListAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetDealerOpening/{DealerOpeningId}")]
        public async Task<IActionResult> GetDealerOpeningListAsync(int DealerOpeningId)
        {
            try
            {
                var result = await _dealerOpeningSvc.GetDealerOpeningDetailById(DealerOpeningId);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPost("CreateDealerOpening")]
        //[RequestSizeLimit(40000000)]
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
                ex.ToWriteLog();
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateDealerOpening")]
        public async Task<IActionResult> UpdateDealerOpeningAsync([FromBody] DealerOpeningModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _dealerOpeningSvc.AppUpdateDealerOpeningAsync(model);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("DeleteDealerOpening/{DealerOpeningId}")]
        public async Task<IActionResult> DeleteDealerOpening(int DealerOpeningId)
        {
            try
            {
                var result = await _dealerOpeningSvc.DeleteDealerOpeningAsync(DealerOpeningId);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}
