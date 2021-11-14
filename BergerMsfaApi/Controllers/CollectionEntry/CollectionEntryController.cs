using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.CollectionEntry;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.CollectionEntry
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CollectionEntryController : BaseController
    {
        private readonly ILogger<CollectionEntryController> _logger;
        private readonly ICollectionEntryService _paymentService;

        public CollectionEntryController(
            ILogger<CollectionEntryController> logger, 
            ICollectionEntryService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("GetCollectionByType")]
        public async Task<IActionResult> GetCollectionByType([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _paymentService.GetCollectionByType(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteCollection/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _paymentService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "Collection is Not Found.");
                    return ValidationResult(ModelState);
                }
                var result = await _paymentService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetCollectionById/{id}")]
        public async Task<IActionResult> GetCollectionById(int id)
        {
            try
            {
                
                var result = await _paymentService.GetCollectionById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        } 
        
        [HttpPost("UpdateCollection")]
        public async Task<IActionResult> UpdateCollection([FromBody] Payment model)
        {
            try
            {
                
                var result = await _paymentService.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
