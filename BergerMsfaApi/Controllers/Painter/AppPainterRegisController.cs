using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpGet("GetPainterList/{employeeId}")]
        public async Task<IActionResult> GetPainterList([BindRequired] string employeeId)
        {
            try
            {
               var result = await _painterSvc.AppGetPainterListAsync(employeeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreatePainter")]
        public async Task<IActionResult> CreatePainter([FromBody]PainterModel model)
        {
            try
            {
   
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _painterSvc.AppCreatePainterAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpGet("GetPainterById/{Id}")]
        //public async Task<IActionResult> GetPainterById([BindRequired] int Id)
        //{
        //    try
        //    {
        //        var result = await _painterSvc.AppGetPainterByIdAsync(Id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("GetPainterByPhone/{Phone}")]
        //public async Task<IActionResult> GetPainterByIPhone([BindRequired] string Phone)
        //{
        //    try
        //    {

        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _painterSvc.AppGetPainterByPhonesync(Phone);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpPut("UpdatePainter")]
        //public async Task<IActionResult> UpdatePainter([FromBody]PainterModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        if (!await _painterSvc.IsExistAsync(model.Id))
        //        {
        //            ModelState.AddModelError(nameof(model.Id), "painter  not found");
        //            return ValidationResult(ModelState);
        //        }
        //        var result = await _painterSvc.AppUpdatePainterAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}



        //[HttpDelete("DeletePainter/{Id}")]
        //public async Task<IActionResult> DeletePainter([BindRequired]int Id)
        //{
        //    try
        //    {
        //        if (!await _painterSvc.IsExistAsync(Id))
        //        {
        //            ModelState.AddModelError(nameof(Id), "painter  not found");
        //            return ValidationResult(ModelState);
        //        }

        //        var result = await _painterSvc.AppDeletePainterByIdAsync(Id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}


    }




}
