using System;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.PainterRegistration1
{

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppPainterRegisController : BaseController
    {
        private readonly IPainterRegistrationService _painterSvc;
        private readonly ILogger<AppPainterRegisController> _logger;
        private readonly IFileUploadService _uploadService;

        public AppPainterRegisController(
            IPainterRegistrationService painterSvc,
            ILogger<AppPainterRegisController> logger,
            IFileUploadService uploadService
            )
        {
            _painterSvc = painterSvc;
            _logger = logger;
            _uploadService = uploadService;
        }

        [HttpGet("GetPainterList")]
        public async Task<IActionResult> GetPainterList()
        {
            try
            {
                var result = await _painterSvc.GetPainterListAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreatePainter")]
        public async Task<IActionResult> CreatePainter([FromBody] PainterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _painterSvc.CreatePainterAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPost("GetPainterById/{Id}")]
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

        [HttpPut("UpdatePainter")]
        public async Task<IActionResult> UpdatePainter([FromBody] PainterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (!await _painterSvc.IsExistAsync(model.Id)) return NotFound();
                var result = await _painterSvc.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                if (!await _painterSvc.IsExistAsync(Id))
                {
                    ModelState.AddModelError(nameof(Id), "Painter Not Found");
                    return ValidationResult(ModelState);
                }
              
                var result = await _painterSvc.DeleteAsync(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("UploadPainterImage/{PainterId}")]
        public async Task<IActionResult> UploadPainterImage(int PainterId, IFormFile file)
        {
            try
            {
                if (!await _painterSvc.IsExistAsync(PainterId))
                {
                    ModelState.AddModelError(nameof(PainterId), "Painter Not Found");
                    return ValidationResult(ModelState);
                }
                if (file==null)
                {
                    ModelState.AddModelError(nameof(file), "File Not Found");
                    return ValidationResult(ModelState);
                }
                 var _painter= await _painterSvc.UploadPainterProfile(PainterId, file);
                return OkResult(_painter);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

    }
}
