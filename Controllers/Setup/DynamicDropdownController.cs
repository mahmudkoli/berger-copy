using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Setup
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DynamicDropdownController : BaseController
    {
        private readonly ILogger<DynamicDropdownController> _logger;
        private readonly IDropdownService _dropdownService;
        public DynamicDropdownController
            (
              ILogger<DynamicDropdownController> logger
             ,IDropdownService dropdownService
            )
        {
            _logger = logger;
            _dropdownService = dropdownService;
        }


        [HttpGet("[action]")]
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

        [HttpGet("[action]/{id}")]
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

        [HttpGet("[action]")]
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


        [HttpGet("[action]/{id}/{typeId}")]
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] DropdownModel detailModel)
        {

            try
            {
             
                var result = await _dropdownService.CreateAsync(detailModel);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] DropdownModel model)
        {

            try
            {
                var isExist = await _dropdownService.IsExistAsync(model);
                if (isExist)
                {
                    var result = await _dropdownService.UpdateAsync(model);
                    return OkResult(result);

                }
                else
                {
                    ModelState.AddModelError(nameof(model.DropdownName), "Does Not Exist");
                    return ValidationResult(ModelState);
                }
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
                var result = await _dropdownService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("[action]/{typeCode}")]
        public async Task<IActionResult> GetDropdownByTypeCd(string typeCode)
        {

            try
            {
                var result = await _dropdownService.GetDropdownByTypeCd(typeCode);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("[action]/{typeId}")]
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
