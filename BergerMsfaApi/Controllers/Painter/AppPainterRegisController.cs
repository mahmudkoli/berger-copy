﻿using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
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
                var result = await _painterSvc.AppGetPainterListAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }




        [HttpPost("CreatePainter")]
        public async Task<IActionResult> CreatePainter(PainterModel model)
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
                var result = await _painterSvc.AppGetPainterByIdAsync(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPut("UpdatePainter")]
        public async Task<IActionResult> UpdatePainter(PainterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (!await _painterSvc.IsExistAsync(model.Id)) return NotFound();
                var result = await _painterSvc.AppUpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }



        [HttpDelete("DeletePainter/{Id}")]
        public async Task<IActionResult> DeletePainter(int Id)
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


    }




}
