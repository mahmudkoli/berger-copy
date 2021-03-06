using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Setup
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDynamicDropdownController : BaseController
    {
        private readonly ILogger<AppDynamicDropdownController> _logger;
        private readonly IDropdownService _dropdownService;
        public AppDynamicDropdownController(
              ILogger<AppDynamicDropdownController> logger, 
              IDropdownService dropdownService)
        {
            _logger = logger;
            _dropdownService = dropdownService;
        }

        [AllowAnonymous]
        [HttpGet("GetDropdownByTypeCd/{typeCode}")]
        public async Task<IActionResult> GetDropdownByTypeCd(string typeCode)
        {
            try
            {
                if(string.IsNullOrEmpty(typeCode))
                {
                    ModelState.AddModelError(nameof(typeCode), "TypeCode Can Not Be Empty");
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

        [AllowAnonymous]
        [HttpGet("GetDropdownByTypeCd")]
        public async Task<IActionResult> GetDropdownByTypeCd([FromQuery] IList<string> typeCodes)
        {
            try
            {
                if (!typeCodes.Any())
                {
                    ModelState.AddModelError(nameof(typeCodes), "TypeCode Can Not Be Empty");
                    return ValidationResult(ModelState);
                }
                var result = await _dropdownService.GetDropdownByTypeCd(typeCodes);
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

        [HttpGet("CompanyList/{painterCallId}")]
        public async Task<IActionResult> CompanyList(int painterCallId)
        {
            try
            {
                var result = await _dropdownService.GetCompanyList(painterCallId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
