using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Scheme
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SchemeDetailController : BaseController
    {
        private readonly ISchemeService _schemeService;

        public SchemeDetailController(
            ISchemeService schemeService)
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _schemeService.GetAllSchemeDetailsAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchemeDetailById(int id)
        {
            try
            {
                var result = await _schemeService.GetSchemeDetailsByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchemeDetail(int id, [FromBody] SaveSchemeDetailModel model)
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

        [HttpDelete("{id}")]
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
