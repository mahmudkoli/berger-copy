using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerSalesCall
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DealerSalesCallController : BaseController
    {
        private readonly IDealerSalesCallService _dealerSalesCallService;

        public DealerSalesCallController(
            IDealerSalesCallService dealerSalesCallService)
        {
            this._dealerSalesCallService = dealerSalesCallService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DealerSalesCallQueryObjectModel query)
        {
            try
            {
                var result = await _dealerSalesCallService.GetAllAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _dealerSalesCallService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPut("UpdateDealerSalesCallList")]
        public async Task<IActionResult> UpdateDealerSalesCallList([FromBody] AppDealerSalesCallModel models)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _dealerSalesCallService.UpdateAsync(models);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("DeleteImage")]
        public async Task<IActionResult> DeleteImage([FromBody] DealerImageModel models)
        {

            try
            {
                await _dealerSalesCallService.DeleteImage(models);
                return OkResult(1);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
