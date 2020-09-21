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

        [HttpGet("[action]/{paymentFrom}")]
        public async Task<IActionResult> GetCollectionByType(string paymentFrom)
        {

            try
            {
                if (string.IsNullOrEmpty(paymentFrom)) return ValidationResult(null);
                var result = await _paymentService.GetCollectionByType(paymentFrom.Trim());
                if (result.Count() == 0)
                {
                    ModelState.AddModelError(nameof(paymentFrom), "does not exist");
                    return ValidationResult(ModelState);
                }
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
