using System;
using System.Security.Principal;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Core;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Journey
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class AppJourneyPlanController : BaseController
    {
        private readonly IJourneyService _journeyService;

        public AppJourneyPlanController(IJourneyService journeyService)
        {
            _journeyService = journeyService;

        }

        [HttpGet("GetJourneyPlanList")]
        public async Task<IActionResult> GetJourneyPlanList()
        {
            try
                
            {
                var result = await _journeyService.GetJourneyPlanList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("ApproveJournetPlan")]
        public async Task<IActionResult> ApproveJournetPlan(JourneyPlanModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.SetApprovedPlan(model);
                return OkResult(result);
            }
            catch (Exception ex)
            { 
                return ExceptionResult(ex);
            };
        }
        [HttpGet("GetJourneyPlanById/{id}")]
        public async Task<IActionResult> GetJourneyPlanById(int id)
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] JourneyPlanModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] JourneyPlanModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                else if (!await _journeyService.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model), "Journey Plan Not Found");
                    return ValidationResult(ModelState);

                }
                var result = await _journeyService.UpdateAsync(model);
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
                if (!await _journeyService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Journey Plan Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _journeyService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
