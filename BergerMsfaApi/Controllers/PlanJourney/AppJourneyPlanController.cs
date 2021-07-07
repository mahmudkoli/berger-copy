using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BergerMsfaApi.Controllers.Journey
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppJourneyPlanController : BaseController
    {
        private readonly IJourneyPlanService _journeyService;

        public AppJourneyPlanController(
            IJourneyPlanService journeyService)
        { 
            _journeyService = journeyService; 
        }

        [HttpGet("GetJourneyPlanList/{employeeId}")]
        public async Task<IActionResult> GetJourneyPlanList([BindRequired]string employeeId)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.AppGetJourneyPlanList(employeeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateJourneyPlan/{employeeId}")]
        public async Task<IActionResult>CreateJourneyPlan([BindRequired] string employeeId,[FromBody] List<AppCreateJourneyModel> model)
        {
            try
            {
                //have to check from app if there is any existing plan create same date and login employee;
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.AppCreateJourneyPlan(employeeId,model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("CheckHasAlreadyPlan/{employeeId}/{visitDate}")]
        public async Task<IActionResult> CheckHasAlreadyPlan([BindRequired] string employeeId, [DataType(DataType.Date)] DateTime visitDate)
        {
            try 
            { 
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                return OkResult(await _journeyService.AppCheckAlreadyTodayPlan(employeeId,visitDate));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLineManagerJourneyPlanList")]
        public async Task<IActionResult> GetJourneyPlanDetailForLineManager()
        {
            try
            {
                var result = await _journeyService.GetAppJourneyPlanListForLineManager();
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
                var result = await _journeyService.GetAppJourneyPlanDetailById(PlanId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("ChangeJourneyPlanStatus")]
        public async Task<IActionResult> ChangePlanStatus(JourneyPlanStatusChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _journeyService.AppChangePlanStatus(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
