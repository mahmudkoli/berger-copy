using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Report;
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

        [HttpGet("GetPainterList")]
        public async Task<IActionResult> GetPainterList([FromQuery] PainterRegistrationReportSearchModel query)
        {
            try
            {
                var result = await _painterSvc.GetPainterListAsync(query);
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

        [HttpPut("UpdatePainterStatus")]
        public async Task<IActionResult> UpdatePainterStatus([FromBody] PainterStatusUpdateModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                else if (!await _painterSvc.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model), "Painter Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _painterSvc.PainterStatusUpdate(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPainterForEdit/{id}")]
        public async Task<IActionResult> UpdatePainter(int id)
        {
            try
            {
                var result = await _painterSvc.GetPainterForEditAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdatePainter")]
        public async Task<IActionResult> UpdatePainter([FromBody] PainterUpdateModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _painterSvc.PainterUpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("DeleteImage")]
        public async Task<IActionResult> DeleteImage([FromBody] PainterImageModel models)
        {
            try
            {
                await _painterSvc.DeleteImage(models);
                return OkResult(true);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeletePainterCall/{id}")]
        public async Task<IActionResult> DeletePainterCall(int id)
        {
            try
            {
                if (!await _painterSvc.IsExistPainterCallAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Painter Call Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _painterSvc.DeletePainterCallAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
