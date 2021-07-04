using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class FocusDealerController : BaseController
    {
        private readonly IFocusDealerService _focusDealerService;
        public FocusDealerController(
            IFocusDealerService focusDealerService)
        {
            _focusDealerService = focusDealerService;
        }

        #region Focus Dealer
        [HttpGet("GetFocusDealerList")]
        public async Task<IActionResult> GetAllAsync([FromQuery] FocusDealerQueryObjectModel query)
        {
            try
            {
                var result = await _focusDealerService.GetAllFocusDealersAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetFocusDealerById/{id}")]
        public async Task<IActionResult> GetFocusDealerById(int id)
        {
            try
            {
                var result = await _focusDealerService.GetFocusDealerById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateFocusDealer")]
        public async Task<IActionResult> Create([FromBody] SaveFocusDealerModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _focusDealerService.CreateFocusDealerAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateFocusDealer")]
        public async Task<IActionResult> Update([FromBody] SaveFocusDealerModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                else if (!await _focusDealerService.IsExistFocusDealerAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model), "Focus Dealer Not Found.");
                    return ValidationResult(ModelState);
                }
                var result = await _focusDealerService.UpdateFocusDealerAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteFocusDealer/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _focusDealerService.IsExistFocusDealerAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Focus Dealer Not Found.");
                    return ValidationResult(ModelState);
                }
                var result = await _focusDealerService.DeleteFocusDealerAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion

        #region Dealer
        [HttpGet("GetDealerList")]
        public async Task<IActionResult> GetDealerList([FromQuery] DealerInfoQueryObjectModel model)
        {
            try
            {
                var result = await _focusDealerService.GetAllDealersAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("UpdateDealerStatus")]
        public async Task<IActionResult> DealerStatusUpdate([FromBody] DealerInfoStatusModel dealer)
        {
            try
            {
                var result = await _focusDealerService.DealerStatusUpdate(dealer);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerInfoStatusLog/{id}")]
        public async Task<IActionResult> GetDealerInfoStatusLogDetails(int id)
        {
            try
            {
                var result = await _focusDealerService.GetDealerInfoStatusLog(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion
    }
}
