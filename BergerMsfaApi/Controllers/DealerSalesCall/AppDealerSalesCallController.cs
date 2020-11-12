using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerSalesCall
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerSalesCallController : BaseController
    {
        private readonly IDealerSalesCallService _dealerSalesCallService;

        public AppDealerSalesCallController(
                IDealerSalesCallService dealerSalesCallService
            )
        {
            this._dealerSalesCallService = dealerSalesCallService;
        }

        [HttpGet("GetDealerSalesCallList")]
        public async Task<IActionResult> GetDealerSalesCallList()
        {
            try
            {
                var result = await _dealerSalesCallService.GetAllAsync(1, int.MaxValue);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerSalesCall/{dealerSalesCallId}")]
        public async Task<IActionResult> GetDealerSalesCall(int dealerSalesCallId)
        {
            try
            {
                var result = await _dealerSalesCallService.GetByIdAsync(dealerSalesCallId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateDealerSalesCall")]
        public async Task<IActionResult> CreateDealerSalesCall([FromForm] SaveDealerSalesCallModel model)
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
    }
}
