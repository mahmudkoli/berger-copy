using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class FocusDealerController : BaseController
    {
        private readonly IFocusDealerService _focusDealerService;
        public FocusDealerController(IFocusDealerService focusDealerService)
        {
            _focusDealerService = focusDealerService;
        }

       
        [HttpGet("GetFocusdealerListPaging/{index}/{pageSize}")]
        public async Task<IActionResult> GetFocusDealerList(int index,int pageSize,string search)
        {
            try
            {
                var result = await _focusDealerService.GetFocusdealerListPaging(index,pageSize, search);
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FocusDealerModel model)
        {

            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _focusDealerService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FocusDealerModel model)
        {

            try
            { 
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                else if (!await _focusDealerService.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model), "Focus Dealer Not Found");
                    return ValidationResult(ModelState);

                }
                var result = await _focusDealerService.UpdateAsync(model);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _focusDealerService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Focus Dealer Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _focusDealerService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerList")]
        public async Task<IActionResult> GetDealerList(int index,int pageSize,string search)
        {
            try
            {
                
                var result = await _focusDealerService.GetDalerListPaging(index,pageSize,search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("UpdateDealerStatus")]
        public async Task<IActionResult> DealerStatusUpdate([FromBody]DealerInfo dealer)
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

    }
}
