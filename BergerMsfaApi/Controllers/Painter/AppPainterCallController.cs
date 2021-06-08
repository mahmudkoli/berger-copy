using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Painter
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppPainterCallController : BaseController
    {
        private readonly IPaintCallService _paintCallSvc;
        public ILogger<AppPainterCallController> _logger;
        public AppPainterCallController(
            ILogger<AppPainterCallController> logger,
            IPaintCallService paintCallSvc
            )
        {
            _logger = logger;
            _paintCallSvc=paintCallSvc;
    }


        //[HttpGet("GetPainterCall")]
        //public async Task<IActionResult> GetPainterCallAsync([BindRequired] string employeeId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _paintCallSvc.AppGetPainterCallListAsync(employeeId);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return ExceptionResult(ex);
        //    }
        //}


        //[HttpGet("GetPainterCallById/{Id}")]
        //public async Task<IActionResult> GetPainterCallByIdAsync(int Id)
        //{
        //    try
        //    {
        //        var result = await _paintCallSvc.AppGetPainterByIdAsync(Id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("GetPainterCallByPainterId/{employeeId}/{PainterId}")]
        //public async Task<IActionResult> GetPainterCallByPainterId([BindRequired] string employeeId,int PainterId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _paintCallSvc.AppGetPainterByPainterIdAsync(employeeId,PainterId);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return ExceptionResult(ex);
        //    }
        //}

        [HttpGet("GetPainterCallByPainterId/{PainterId}")]
        public async Task<IActionResult> CreatePainterCallAysnc([BindRequired] int PainterId)
        {
            try
            {
                var result = await _paintCallSvc.AppCreatePainterCallAsync(PainterId);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpPost("CreatePainterCall/{employeeId}")]
        public async Task<IActionResult> CreatePainterCallAysnc([BindRequired] string employeeId, [FromBody] PainterCallModel model)
        {
            try
            {
                var result = await _paintCallSvc.AppCreatePainterCallAsync(employeeId, model);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        //[HttpPut("UpdatePainterCall")]
        //public async Task<IActionResult> UpdatePainterCallAysnc([BindRequired] string employeeId,[FromBody] PainterCallModel model)
        //{
        //    try
        //    {
        //        if(!await _paintCallSvc.IsExistAsync(model.Id))
        //        {
        //            ModelState.AddModelError(nameof(model.Id), "Painter Call Not Found");
        //            return ValidationResult(ModelState);
        //        }
        //        var result = await _paintCallSvc.AppUpdatePainterCallAsync(employeeId,model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpDelete("DeletePainterCallById/{PainterId}")]
        //public async Task<IActionResult> DeletePainterCallById(int PainterId)
        //{
        //    try
        //    {
        //        var result = await _paintCallSvc.DeletePainterCallByIdlAsync(PainterId);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return ExceptionResult(ex);
        //    }
        //}

    }
}
