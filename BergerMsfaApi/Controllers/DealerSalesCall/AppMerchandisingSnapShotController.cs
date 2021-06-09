using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.MerchandisingSnapShot;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.MerchandisingSnapShot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.MerchandisingSnapShot
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppMerchandisingSnapShotController : BaseController
    {
        private readonly IMerchandisingSnapShotService _merchandisingSnapShotService;

        public AppMerchandisingSnapShotController(
                IMerchandisingSnapShotService merchandisingSnapShotService
            )
        {
            this._merchandisingSnapShotService = merchandisingSnapShotService;
        }

        [HttpGet("MerchandisingSnapShotList")]
        public async Task<IActionResult> GetMerchandisingSnapShotList([FromQuery] int dealerId)
        {
            try
            {
                var result = await _merchandisingSnapShotService.GetAppMerchandisingSnapShotListByCurrentUser(dealerId);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        //[HttpGet("GetMerchandisingSnapShotByDealerId/{id}")]
        //public async Task<IActionResult> GetMerchandisingSnapShotByDealerId(int id)
        //{
        //    try
        //    {
        //        var result = await _merchandisingSnapShotService.GetMerchandisingSnapShotByDealerIdAsync(id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("GetMerchandisingSnapShotListByDealerIds")]
        //public async Task<IActionResult> GetMerchandisingSnapShotListByDealerIds([FromQuery] IList<int> ids)
        //{
        //    try
        //    {
        //        var result = await _merchandisingSnapShotService.GetMerchandisingSnapShotListByDealerIdsAsync(ids);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        [HttpPost("CreateMerchandisingSnapShot")]
        public async Task<IActionResult> CreateMerchandisingSnapShot([FromBody] SaveMerchandisingSnapShotModel model)
        {
            if (!ModelState.IsValid)
            {
                return AppValidationResult(ModelState);
            }

            try
            {
                var result = await _merchandisingSnapShotService.AddAsync(model);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpPost("CreateMerchandisingSnapShotList")]
        public async Task<IActionResult> CreateMerchandisingSnapShotList([FromBody] List<SaveMerchandisingSnapShotModel> models)
        {
            if (!ModelState.IsValid)
            {
                return AppValidationResult(ModelState);
            }

            try
            {
                var result = await _merchandisingSnapShotService.AddRangeAsync(models);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }
    }
}
