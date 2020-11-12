using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Scheme
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SchemeDetailController : BaseController
    {
        private readonly ISchemeService _schemeService;

        public SchemeDetailController(ISchemeService schemeService)
        {
            _schemeService = schemeService;

        }

        [HttpGet("GetSchemeDetailWithMaster")]
        public async Task<IActionResult> GetSchemeDetailWithMaster()
        {
            try
            {
                var result = await _schemeService.PortalGetcShemeDetailWithMaster();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetSchemeDetailList")]
        public async Task<IActionResult> GetSchemeDetailList()
        {
            try
            {
                var result = await _schemeService.PortalGetSchemeDelails();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSchemeDetailById/{Id}")]
        public async Task<IActionResult> GetSchemeDetailById(int Id)
        {
            try
            {
                var result = await _schemeService.PortalGetSchemeDetailById(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateSchemeDetail")]
        public async Task<IActionResult> CreateSchemeDetail([FromBody] SchemeDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.PortalCreateSchemeDeatil(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpPut("UpdateSchemeDetail")]
        public async Task<IActionResult> UpdateSchemeDetail([FromBody] SchemeDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.PortalUpdateSchemeDetail(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpDelete("DeleteSchemeDetail/{Id}")]
        public async Task<IActionResult> DeleteSchemeDetail(int Id)
        {
            try
            {
                var exist = await _schemeService.IsSchemeDetailAlreadyExist(Id);
                if (exist == false)
                {
                    ModelState.AddModelError(nameof(Id), "Scheme detail not exist");
                    return ValidationResult(ModelState);

                }
                var result = await _schemeService.PortalDeleteSchemeDetail(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
    }
}
