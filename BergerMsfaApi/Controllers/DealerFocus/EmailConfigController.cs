using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class EmailConfigController : BaseController
    {
        private ILogger<EmailConfigController> _logger;
        private readonly IEmailConfigService  _emailConfigService;

        public EmailConfigController(
            ILogger<EmailConfigController> logger, 
            IEmailConfigService emailConfigService)
        {
            _logger = logger;
            _emailConfigService = emailConfigService;
        }

        [HttpGet("GetEmailConfig")]
        public async Task<IActionResult> GetEmailConfig()
        {
            try
            {
                var result = await _emailConfigService.GetEmailConfig();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpPost("CreateEmailConfig")]
        public async Task<IActionResult> CreateEmailConfigAsync([FromBody] EmailConfigForDealerOppening model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _emailConfigService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateEmailConfig")]
        public async Task<IActionResult> UpdateEmailConfigAsync([FromBody] EmailConfigForDealerOppening model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _emailConfigService.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _emailConfigService.GetById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        } 
        
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await _emailConfigService.DeleteDealerOppeningEmailById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
