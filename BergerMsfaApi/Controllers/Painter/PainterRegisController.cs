using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.PainterRegistration
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v1:apiVersion}/[controller]")]
    public class PainterRegisController : BaseController
    {
        private readonly IPainterRegistrationService _painterSvc;

        public PainterRegisController(
            IPainterRegistrationService painterSvc)
        {
            _painterSvc = painterSvc;
        }

        [HttpGet("GetPainterList/{index}/{pageSize}")]
        public async Task<IActionResult> GetPainterList(int index, int pageSize, string search)
        {
            try
            {
                var result = await _painterSvc.GetPainterListAsync(index,pageSize,search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPainterById/{Id}")]
        public async Task<IActionResult> GetPainterById(int Id)
        {
            try
            {
                if (!await _painterSvc.IsExistAsync(Id))
                {
                    ModelState.AddModelError(nameof(Id), "Painter Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _painterSvc.GetPainterByIdAsync(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("UpdatePainterStatus/{Id}")]
        public async Task<IActionResult> UpdatePainterStatus(int Id)
        {
            try
            {
                var result = await _painterSvc.PainterStatusUpdate(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
