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
    public class SchemeMasterController : BaseController
    {
        private readonly ISchemeService _schemeService;

        public SchemeMasterController(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        [HttpGet("GetSchemeMasterList/{index}/{pageSize}")]
        public async Task<IActionResult> GetSchemeMasterList(int index, int pageSize, string search)
        {
            try
            {
                var result = await _schemeService.GetAllSchemeMastersAsync(index, pageSize, search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSchemeMasterById/{Id}")]
        public async Task<IActionResult> GetSchemeMasterById(int Id)
        {
            try
            {
                var result = await _schemeService.GetSchemeMasterByIdAsync(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateSchemeMaster")]
        public async Task<IActionResult> CreateSchemeMaster([FromBody] SaveSchemeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.AddSchemeMasterAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateSchemeMaster")]
        public async Task<IActionResult> UpdateSchemeMaster([FromBody] SaveSchemeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.UpdateSchemeMasterAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteSchemeMaster/{id}")]
        public async Task<IActionResult> DeleteSchemeMaster(int id)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.DeleteSchemeMasterAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetAllSchemeMastersForSelect")]
        public async Task<IActionResult> GetAllSchemeMastersForSelect()
        {
            try
            {
                var result = await _schemeService.GetAllSchemeMastersForSelectAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
