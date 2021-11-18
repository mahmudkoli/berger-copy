using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Setup
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DynamicDropdownController : BaseController
    {
        private readonly ILogger<DynamicDropdownController> _logger;
        private readonly IDropdownService _dropdownService;
        public DynamicDropdownController(
              ILogger<DynamicDropdownController> logger, 
              IDropdownService dropdownService)
        {
            _logger = logger;
            _dropdownService = dropdownService;
        }


        [HttpGet("GetDropdownList")]
        public async Task<IActionResult> GetDropdownList()
        {
            try
            {
                var result = await _dropdownService.GetDropdownList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDropdownListPaging")]
        public async Task<IActionResult> GetDropdownListPaging(int index,int pageSize)
        {
            try
            {
                var result = await _dropdownService.GetDropdownListPaging(index, pageSize);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDropdownById/{id}")]
        public async Task<IActionResult> GetDropdownById(int id)
        {
            try
            {
                var result = await _dropdownService.GetDropdownById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDropdownTypeList")]
        public async Task<IActionResult> GetDropdownTypeList()
        {
            try
            {
                var result = await _dropdownService.GetDropdownTypeList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLastSquence/{id}/{typeId}")]
        public async Task<IActionResult> GetLastSquence(int id,int typeId)
        {
            try
            {
                var result = await _dropdownService.GetLastSquence(id,typeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DropdownModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _dropdownService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] DropdownModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                else if (!await _dropdownService.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model), "Dropdown Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _dropdownService.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _dropdownService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Dropdown  Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _dropdownService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDropdownByTypeCd/{typeCode}")]
        public async Task<IActionResult> GetDropdownByTypeCd(string typeCode)
        {
            try
            {
                if (string.IsNullOrEmpty(typeCode))
                {
                    ModelState.AddModelError(nameof(typeCode), "TypeCode Could Not Be Empty");
                    return ValidationResult(ModelState);
                }
                var result = await _dropdownService.GetDropdownByTypeCd(typeCode.Trim());
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDropdownByTypeId/{typeId}")]
        public async Task<IActionResult> GetDropdownByTypeId(int typeId)
        {
            try
            {
                var result = await _dropdownService.GetDropdownByTypeId(typeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
