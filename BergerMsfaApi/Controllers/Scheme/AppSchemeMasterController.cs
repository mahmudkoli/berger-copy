using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Scheme
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppSchemeMasterController : BaseController
    {
        private readonly ISchemeService _schemeService;

        public AppSchemeMasterController(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        //[HttpGet("GetSchemeMasterList")]
        //public async Task<IActionResult> GetSchemeMasterList()
        //{
        //    try
        //    {
        //        var result = await _schemeService.GetAllSchemeMastersAsync();
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("GetSchemeMasterById/{id}")]
        //public async Task<IActionResult> GetSchemeMasterById(int id)
        //{
        //    try
        //    {
        //        var result = await _schemeService.GetSchemeMasterByIdAsync(id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpPost("CreateSchemeMaster")]
        //public async Task<IActionResult> CreateSchemeMaster([FromBody] SaveSchemeMasterModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.AddSchemeMasterAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpPut("UpdateSchemeMaster")]
        //public async Task<IActionResult> UpdateSchemeMaster([FromBody] SaveSchemeMasterModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.UpdateSchemeMasterAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpDelete("DeleteSchemeMaster/{id}")]
        //public async Task<IActionResult> DeleteSchemeMaster(int id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.DeleteSchemeMasterAsync(id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}
    }
}
