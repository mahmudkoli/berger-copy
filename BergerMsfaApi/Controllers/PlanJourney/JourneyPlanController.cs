using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Journey
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class JourneyPlanController : BaseController
    {
        private readonly IJourneyPlanService _journeyService;
        public JourneyPlanController(IJourneyPlanService journeyService) => _journeyService = journeyService;

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
        public async Task<IActionResult> GetJourneyPlanListPaging(int index, int pageSize, string planDate)
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanDetailPaging(index, pageSize, planDate);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetJourneyPlanListPaging/{index}/{pageSize}")]
        public async Task<IActionResult> GetJourneyPlanListPaging(int index,int pageSize)
        {
            try
            {
                var result = await _journeyService.PortalGetJourneyPlanDeailPage(index,pageSize);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetLineManagerJourneyPlanDetail")]
        public async Task<IActionResult> GetJourneyPlanDetailForLineManager()
        {
            try
            {
                var result = await _journeyService.GetJourneyPlanDetailForLineManager();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        
        [HttpGet("GetJourneyPlanById/{id}")]
        public async Task<IActionResult> GetJourneyPlanById(int id)
        {
            try
            {
                var result = await _journeyService.PortalGetJourneyPlanById(id);
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
                if (await _journeyService.CheckAlreadyTodayPlan())
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
