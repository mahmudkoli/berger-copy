using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerSalesCall
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerSalesCallController : BaseController
    {
        private readonly IDealerSalesCallService _dealerSalesCallService;

        public AppDealerSalesCallController(
            IDealerSalesCallService dealerSalesCallService)
        {
            _dealerSalesCallService = dealerSalesCallService;
        }

        [HttpGet("GetDealerSalesCallByDealerId/{id}")]
        public async Task<IActionResult> GetDealerSalesCallByDealerId(int id)
        {
            try
            {
                var result = await _dealerSalesCallService.GetDealerSalesCallByDealerIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerSalesCallListByDealerIds")]
        public async Task<IActionResult> GetDealerSalesCallListByDealerIds([FromQuery] IList<int> ids)
        {
            try
            {
                var result = await _dealerSalesCallService.GetDealerSalesCallListByDealerIdsAsync(ids);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateDealerSalesCall")]
        public async Task<IActionResult> CreateDealerSalesCall([FromBody] SaveDealerSalesCallModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _dealerSalesCallService.AddAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateDealerSalesCallList")]
        public async Task<IActionResult> CreateDealerSalesCallList([FromBody] List<SaveDealerSalesCallModel> models)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _dealerSalesCallService.AddRangeAsync(models);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


    }
}
