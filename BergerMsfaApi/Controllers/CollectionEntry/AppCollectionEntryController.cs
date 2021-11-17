using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.CollectionEntry
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppCollectionEntryController : BaseController
    {
        private readonly ILogger<AppCollectionEntryController> _logger;
        private readonly ICollectionEntryService _paymentService;

        public AppCollectionEntryController(
            ILogger<AppCollectionEntryController> logger, 
            ICollectionEntryService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("GetCollectionList")]
        public async Task<IActionResult> GetCollectionList()
        {
            try
            {
                var result = await _paymentService.GetAppCollectionListByCurrentUserAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentModel model)
        {
            try
            {
                if (!DateTime.TryParse(model.CollectionDate, out DateTime date))
                {
                    ModelState.AddModelError(nameof(model.CollectionDate), "Collection date is not valid format.");
                    return ValidationResult(ModelState);
                }
                    
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _paymentService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetCreditControlArea")]
        public async Task<IActionResult> GetCreditControlArea()
        {
            try
            {
                var result = await _paymentService.GetCreditControlAreaList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
