using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Painter
{
    [AuthorizeFilter]
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

        [HttpGet("GetPainterCallByPainterId/{PainterId}")]
        public async Task<IActionResult> GetPainterCallByPainterIdAysnc([BindRequired] int PainterId)
        {
            try
            {
                var result = await _paintCallSvc.AppCreatePainterCallAsync(PainterId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreatePainterCall/{employeeId}")]
        public async Task<IActionResult> CreatePainterCallAysnc([BindRequired] string employeeId, [FromBody] PainterCallModel model)
        {
            try
            {
                if (!await _paintCallSvc.IsExistCurrentDays(model.PainterId))
                {
                    ModelState.AddModelError(nameof(model.PainterId), "Today this painter already call");
                    return ValidationResult(ModelState);
                }
                var result = await _paintCallSvc.AppCreatePainterCallAsync(employeeId, model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
