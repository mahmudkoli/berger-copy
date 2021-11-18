using Berger.Data.MsfaEntity.DemandGeneration;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DemandGeneration
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class LeadController : BaseController
    {
        private readonly ILeadService _leadService;

        public LeadController(
            ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] LeadGenerationDetailsReportSearchModel query)
        {
            try
            {
                var result = await _leadService.GetAllAsync(query);
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
                var result = await _leadService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLeadById/{id}")]
        public async Task<IActionResult> GetLeadById(int id)
        {
            try
            {
                var result = await _leadService.GetLeadByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("UpdateLeadGenerate")]
        public async Task<IActionResult> UpdateLeadGenerate([FromBody] UpdateLeadGenerationModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _leadService.UpdateLeadGenerateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteLeadFollowUp/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _leadService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Lead Follow Up Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _leadService.DeleteAsync(id);
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
                await _leadService.DeleteImage(models);
                return OkResult(1);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
