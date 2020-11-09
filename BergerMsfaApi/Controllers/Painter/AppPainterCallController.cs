using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Painter
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v1{v:apiVersion}/[controller]")]
    public class AppPainterCallController : BaseController
    {
        private readonly IPaintCallService _paintCallSvc;
        public ILogger<AppPainterCallController> _logger;
        public AppPainterCallController(ILogger<AppPainterCallController> logger, IPaintCallService paintCallSvc)
        {
            _logger = logger;
            _paintCallSvc=paintCallSvc;
    }


        [HttpGet("GetPainterCall")]
        public async Task<IActionResult> GetPainterCallAsync()
        {
            try
            {
                var result = await _paintCallSvc.AppGetPainterCallListAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPainterCallById/{PainterId}")]
        public async Task<IActionResult> GetPainterCallByIdAsync(int PainterId)
        {
            try
            {
                var result = await _paintCallSvc.AppGetPainterByIdAsync(PainterId);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreatePainterCall")]
        public async Task<IActionResult> CreatePainterCallAysnc([FromBody] PainterCallModel model)
        {
            try
            {
                var result = await _paintCallSvc.AppCreatePainterCallAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdatePainterCall")]
        public async Task<IActionResult> UpdatePainterCallAysnc([FromBody] PainterCallModel model)
        {
            try
            {
                if(!await _paintCallSvc.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model.Id), "Painter Call Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _paintCallSvc.AppUpdatePainterCallAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpDelete("DeletePainterCallById/{PainterId}")]
        public async Task<IActionResult> DeletePainterCallById(int PainterId)
        {
            try
            {
                var result = await _paintCallSvc.DeletePainterCallByIdlAsync(PainterId);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

    }
}
