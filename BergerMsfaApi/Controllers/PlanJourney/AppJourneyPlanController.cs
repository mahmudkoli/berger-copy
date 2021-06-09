﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BergerMsfaApi.Controllers.Journey
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class AppJourneyPlanController : BaseController
    {
        private readonly IJourneyPlanService _journeyService;

        public AppJourneyPlanController
            (
            IJourneyPlanService journeyService
            )=> _journeyService = journeyService;

        //this method expose journey plan list by employeeId

        [HttpGet("GetJourneyPlanList/{employeeId}")]
        public async Task<IActionResult> GetJourneyPlanList([BindRequired]string employeeId)
        {
            try
            {
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _journeyService.AppGetJourneyPlanList(employeeId);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpPost("CreateJourneyPlan/{employeeId}")]
        public async Task<IActionResult>CreateJourneyPlan([BindRequired] string employeeId,[FromBody] List<AppCreateJourneyModel> model)
        {
            try
            {
                //have to check from app if there is any existing plan create same date and login employee;
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _journeyService.AppCreateJourneyPlan(employeeId,model);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("CheckHasAlreadyPlan/{employeeId}/{visitDate}")]
        public async Task<IActionResult> CheckHasAlreadyPlan([BindRequired] string employeeId, [DataType(DataType.Date)] DateTime visitDate)
        {
            try { 
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                return AppOkResult(await _journeyService.AppCheckAlreadyTodayPlan(employeeId,visitDate));
            }
            catch (Exception ex)
            {

                return AppExceptionResult(ex);
            }
           

        }

        [HttpGet("GetLineManagerJourneyPlanList")]
        public async Task<IActionResult> GetJourneyPlanDetailForLineManager()
        {
            try
            {
                var result = await _journeyService.GetAppJourneyPlanListForLineManager();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetJourneyPlanDetailById/{PlanId}")]
        public async Task<IActionResult> GetJourneyPlanDetailById(int PlanId)
        {
            try
            {
                var result = await _journeyService.GetAppJourneyPlanDetailById(PlanId);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpPost("ChangeJourneyPlanStatus")]
        public async Task<IActionResult> ChangePlanStatus(JourneyPlanStatusChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _journeyService.AppChangePlanStatus(model);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            };
        }

        //[HttpPost("UpdateJourneyPlan/{employeeId}")]
        //public async Task<IActionResult> UpdateJourneyPlan([BindRequired]string employeeId, [FromBody] List<AppCreateJourneyModel> model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _journeyService.AppUpdateJourneyPlan(employeeId, model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}


        //[HttpDelete("DeleteJourneyPlan")]
        //public async Task<IActionResult> DeleteJourneyPlan([BindRequired]int PlanId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _journeyService.DeleteJourneyAsync(PlanId);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("GetJouneyPlanDealerList/{employeeId}")]
        //public async Task<IActionResult> GetJouneyPlanDealerList([BindRequired] string employeeId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _journeyService.AppGetJourneyPlanDealerList(employeeId);
        //        return OkResult(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }

        //}
    }
}
