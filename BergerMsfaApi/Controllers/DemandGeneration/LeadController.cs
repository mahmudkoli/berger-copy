using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DemandGeneration
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class LeadController : BaseController
    {
        private readonly ILeadService _leadService;

        public LeadController(
            ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _leadService.GetAllAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _leadService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteLeadFollowUp/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _leadService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Focus Dealer Not Found");
                    return ValidationResult(ModelState);
                }
                var result = await _leadService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
