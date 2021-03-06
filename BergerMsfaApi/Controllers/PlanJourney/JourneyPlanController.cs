using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BergerMsfaApi.Controllers.Journey
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class JourneyPlanController : BaseController
    {
        private readonly IJourneyPlanService _journeyService;

        public JourneyPlanController(
            IJourneyPlanService journeyService) 
        { 
            _journeyService = journeyService; 
        }

        [HttpGet("GetJourneyPlanList")]
        public async Task<IActionResult> GetJourneyPlanList()
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanDetail();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetJourneyPlanListPaging/{index}/{pageSize}")]
        public async Task<IActionResult> GetJourneyPlanListPaging([BindRequired] int index, int pageSize, string search)
        {
            try
            {
                var result = await _journeyService.PortalGetJourneyPlanDeailPage(index,pageSize, search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLineManagerJourneyPlanDetail/{index}/{pageSize}")]
        public async Task<IActionResult> GetJourneyPlanDetailForLineManager(int index,int pageSize,string search)
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanDetailForLineManager( index,  pageSize, search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpGet("GetJourneyPlanById/{date}")]
        public async Task<IActionResult> GetJourneyPlanById(string date)
        {
            try
            {
                var result = await _journeyService.PortalGetJourneyPlanById(date);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetJourneyPlanDetailById/{PlanId}")]
        public async Task<IActionResult> GetJourneyPlanDetailById(int PlanId)
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanDetailById(PlanId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("ChangeJourneyPlanStatus")]
        public async Task<IActionResult> ChangePlanStatus(JourneyPlanStatusChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.ChangePlanStatus(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            };
        }
  
        [HttpPost("CreateJourneyPlan")]
        public async Task<IActionResult> CreateJourneyPlan([FromBody] PortalCreateJouneryModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (await _journeyService.CheckAlreadyTodayPlan(model.VisitDate))
                {
                    ModelState.AddModelError("Plan", "you have already created today's plan");
                    return ValidationResult(ModelState);
                }
                var result = await _journeyService.PortalCreateJourneyPlan(model);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateJourneyPlan")]
        public async Task<IActionResult> UpdateJourneyPlan([FromBody] PortalCreateJouneryModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.PortalUpdateJourneyPlan(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteJourneyPlan/{PlanId}")]
        public async Task<IActionResult> DeleteJourneyPlan(int PlanId)
        {
            try
            {
               if(!await _journeyService.IsExistAsync(PlanId))
                {
                    ModelState.AddModelError(nameof(PlanId), "Jounery Plan Not found");
                    return ValidationResult(ModelState);
                }
                var result = await _journeyService.DeleteJourneyAsync(PlanId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
