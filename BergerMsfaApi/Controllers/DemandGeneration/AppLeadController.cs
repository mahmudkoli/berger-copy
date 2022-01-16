using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DemandGeneration
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppLeadController : BaseController
    {
        private readonly ILeadService _leadService;

        public AppLeadController(
            ILeadService leadService)
        {
            _leadService = leadService;
        }

        // updated - area wise data get, not user wise
        [HttpGet("GetAllByUserId/{id}")]
        public async Task<IActionResult> GetAllByUserId(int id)
        {
            try
            {
                var result = await _leadService.GetAllPendingProjectByUserIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLeadFollowUpByLeadGenerateId/{id}")]
        public async Task<IActionResult> GetLeadFollowUpByLeadGenerateId(int id)
        {
            try
            {
                var result = await _leadService.GetLeadFollowUpByLeadGenerateIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateLeadGenerate")]
        public async Task<IActionResult> CreateLeadGenerate([FromBody] AppSaveLeadGenerationModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _leadService.AddLeadGenerateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateLeadFollowUp")]
        public async Task<IActionResult> CreateLeadFollowUp([FromBody] AppSaveLeadFollowUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _leadService.AddLeadFollowUpAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
