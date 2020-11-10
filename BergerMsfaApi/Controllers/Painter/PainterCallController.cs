using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Painter
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v1:apiVersion}/[controller]")]

    public class PainterCallController : BaseController
    {
        private readonly IPaintCallService _paintCallSvc;
        private readonly ILogger<PainterCallController> _logger;
        public PainterCallController(IPaintCallService paintCallSvc, ILogger<PainterCallController> logger)
        {
            _paintCallSvc = paintCallSvc;
            _logger = logger;
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


    }
}
