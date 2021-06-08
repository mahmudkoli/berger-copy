using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Tinting;
using BergerMsfaApi.Services.Tinting.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Tinting
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppTintingMachineController : BaseController
    {
        private readonly ILogger<AppTintingMachineController> _logger;
        private readonly ITintiningService _tintiningService;
        public AppTintingMachineController
            (
            ILogger<AppTintingMachineController> logger,
            ITintiningService tintiningService
            )
        {
            _tintiningService = tintiningService;
            _logger = logger;
        }
      
        [HttpGet("GetTintingMachineList")]
        public async Task< IActionResult> GetTintingMachineList([FromQuery]string territory, [FromQuery]int userInfoId)
        {
            try
            {
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _tintiningService.GetAllAsync(territory, userInfoId);
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpPost("UpdateTintingMachineList")]
        public async Task<IActionResult> UpdateTintingMachineList([FromBody] List<SaveTintingMachineModel> model)
        {
            try
            {
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _tintiningService.UpdateAsync(model);
                return AppOkResult(result);
            }
            catch (System.Exception ex)
            {

                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetAllTintingMachineList")]
        public async Task<IActionResult> GetTintingMachineList([FromQuery] AppTintingMachineSearchModel query)
        {
            try
            {
                if (!ModelState.IsValid) return AppValidationResult(ModelState);
                var result = await _tintiningService.GetAllAsync(query);
                return AppOkResult(result);
            }
            catch (System.Exception ex)
            {

                return AppExceptionResult(ex);
            }
        }
    }
}
