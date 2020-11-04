using System;
using System.Collections.Generic;
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

    public class AppJourneyPlanController : BaseController
    {
        private readonly IJourneyPlanService _journeyService;

        public AppJourneyPlanController(IJourneyPlanService journeyService)
        {
            _journeyService = journeyService;

        }

        //this method expose journey plan list by employeeId

        [HttpGet("GetJourneyPlanList/{employeeId}")]
        public async Task<IActionResult> GetJourneyPlanList(int employeeId)
        {
            try
            {
                var result = await _journeyService.AppGetJourneyPlanDetailList(employeeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateJourneyPlan")]
        public async Task<IActionResult>CreateJourneyPlan([FromBody] List<AppCreateJourneyModel> model)
        {
            try
            {
                //have to check from app if there is any existing plan create same date and login employee;
                if (!ModelState.IsValid) return ValidationResult(ModelState);

                var result = await _journeyService.AppCreateJourneyPlan(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("CheckHasAlreadyPlan/{employeeId}/{planDate}")]
        public async Task<IActionResult> CheckHasAlreadyPlan(int employeeId, string planDate)
        {
            return OkResult(await _journeyService.AppCheckAlreadyTodayPlan(employeeId, DateTime.Parse(planDate)));

        }

        [HttpPost("UpdateJourneyPlan")]
        public async Task<IActionResult> UpdateJourneyPlan([FromBody] List<AppCreateJourneyModel> model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.AppUpdateJourneyPlan(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpDelete("DeleteJourneyPlan")]
        public async Task<IActionResult> DeleteJourneyPlan(int PlanId)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
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
