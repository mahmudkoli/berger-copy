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


        [HttpGet("GetSchemeMasterList")]
        public async Task<IActionResult> GetSchemeMasterList()
        {
            try
            {
              var result=  await _schemeService.PortalGetSchemeMasters();
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
                var result = await _schemeService.PortalGetSchemeMastersById(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateSchemeMaster")]
        public async Task<IActionResult> CreateSchemeMaster([FromBody] SchemeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.PortalCreateSchemeMasters(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpPut("UpdateSchemeMaster")]
        public async Task<IActionResult> UpdateSchemeMaster([FromBody] SchemeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.PortalUpdateSchemeMasters(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpDelete("DeleteSchemeMaster/{Id}")]
        public async Task<IActionResult> DeleteSchemeMaster(int Id)
        {
            try
            {
                var exist = await _schemeService.IsSchemeMasterAlreadyExist(Id);
                if (exist == false)
                {
                    ModelState.AddModelError(nameof(Id), "Scheme master not exist");
                    return ValidationResult(ModelState);

                }
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.PortalDeleteSchemeMasters(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
    }
}
