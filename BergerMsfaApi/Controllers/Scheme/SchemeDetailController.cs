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

        [HttpGet("GetSchemeDetailList/{index}/{pageSize}")]
        public async Task<IActionResult> GetSchemeDetailWithMaster(int index,int pageSize,string search)
        {
            try
            {
                var result = await _schemeService.GetAllSchemeDetailsAsync(index, pageSize, search);
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
                var result = await _schemeService.GetSchemeDetailsByIdAsync(Id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("CreateSchemeDetail")]
        public async Task<IActionResult> CreateSchemeDetail([FromBody] SaveSchemeDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.AddSchemeDeatilsAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateSchemeDetail")]
        public async Task<IActionResult> UpdateSchemeDetail([FromBody] SaveSchemeDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _schemeService.UpdateSchemeDetailsAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteSchemeDetail/{id}")]
        public async Task<IActionResult> DeleteSchemeDetail(int id)
        {
            try
            {
                var result = await _schemeService.DeleteSchemeDetailsAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
