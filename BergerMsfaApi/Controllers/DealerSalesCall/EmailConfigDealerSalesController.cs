using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class EmailConfigDealerSalesController : BaseController
    {
        private ILogger<EmailConfigDealerSalesController> _logger;
        private readonly IEmailConfigService  _emailConfigService;
        public EmailConfigDealerSalesController(ILogger<EmailConfigDealerSalesController> logger
            , IEmailConfigService emailConfigService)
        {
            _logger = logger;
            _emailConfigService = emailConfigService;
        }

        [HttpGet("GetEmailConfig")]
        public async Task<IActionResult> GetEmailConfig()
        {
            try
            {
                var result = await _emailConfigService.GetEmailConfigDealerSalesCall();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpPost("CreateEmailConfig")]
        //[RequestSizeLimit(40000000)]
        public async Task<IActionResult> CreateEmailConfigAsync([FromBody] EmailConfigForDealerSalesCall model)
        {
            try
            {

                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _emailConfigService.CreateDealerSalesCallAsync(model);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                ex.ToWriteLog();
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateEmailConfig")]
        public async Task<IActionResult> UpdateEmailConfigAsync([FromBody] EmailConfigForDealerSalesCall model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _emailConfigService.UpdateDealerSalesCallAsync(model);
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
                var result = await _emailConfigService.GetByIdDealerSalesCall(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }



    }
}
