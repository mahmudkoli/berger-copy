using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.CollectionEntry
{

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CollectionEntryController : BaseController
    {
        private readonly ILogger<CollectionEntryController> _logger;
        private readonly ICollectionEntryService _paymentService;

        public CollectionEntryController(ILogger<CollectionEntryController> logger
            , ICollectionEntryService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("GetCollectionByType/{paymentFrom}")]
        public async Task<IActionResult> GetCollectionByType(string paymentFrom)
        {

            try
            {
                if (string.IsNullOrEmpty(paymentFrom)) {
                    ModelState.AddModelError(nameof(paymentFrom), "Payment Can Not Be Empty");
                    return ValidationResult(ModelState);
                }
                var result = await _paymentService.GetCollectionByType(paymentFrom.Trim());
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
