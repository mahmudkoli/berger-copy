using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Scheme
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppSchemeDetailController : BaseController
    {

        private readonly ISchemeService _schemeService;

        public AppSchemeDetailController(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        [HttpGet("GetSchemeDetailList")]
        public async Task<IActionResult> GetSchemeDetailList()
        {
            try
            {
                var result = await _schemeService.GetAppAllSchemeDetailsByCurrentUserAsync();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        //[HttpGet("GetSchemeDetailById/{id}")]
        //public async Task<IActionResult> GetSchemeDetailById(int id)
        //{
        //    try
        //    {
        //        var result = await _schemeService.GetSchemeDetailsByIdAsync(id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpPost("CreateSchemeDetail")]
        //public async Task<IActionResult> CreateSchemeDetail([FromBody] SaveSchemeDetailModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.AddSchemeDeatilsAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpPut("UpdateSchemeDetail")]
        //public async Task<IActionResult> UpdateSchemeDetail([FromBody] SaveSchemeDetailModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.UpdateSchemeDetailsAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpDelete("DeleteSchemeDetail/{id}")]
        //public async Task<IActionResult> DeleteSchemeDetail(int Id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _schemeService.DeleteSchemeDetailsAsync(Id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}
    }
}
