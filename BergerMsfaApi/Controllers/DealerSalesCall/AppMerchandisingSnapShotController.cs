using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.MerchandisingSnapShot;
using BergerMsfaApi.Services.MerchandisingSnapShot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.MerchandisingSnapShot
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppMerchandisingSnapShotController : BaseController
    {
        private readonly IMerchandisingSnapShotService _merchandisingSnapShotService;

        public AppMerchandisingSnapShotController(
            IMerchandisingSnapShotService merchandisingSnapShotService)
        {
            this._merchandisingSnapShotService = merchandisingSnapShotService;
        }

        [HttpGet("MerchandisingSnapShotList")]
        public async Task<IActionResult> GetMerchandisingSnapShotList([FromQuery] int dealerId)
        {
            try
            {
                var result = await _merchandisingSnapShotService.GetAppMerchandisingSnapShotListByCurrentUser(dealerId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateMerchandisingSnapShot")]
        public async Task<IActionResult> CreateMerchandisingSnapShot([FromBody] SaveMerchandisingSnapShotModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _merchandisingSnapShotService.AddAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateMerchandisingSnapShotList")]
        public async Task<IActionResult> CreateMerchandisingSnapShotList([FromBody] List<SaveMerchandisingSnapShotModel> models)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _merchandisingSnapShotService.AddRangeAsync(models);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
