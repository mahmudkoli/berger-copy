using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.CollectionEntry
{

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppCollectionEntryController : BaseController
    {
        private readonly ILogger<AppCollectionEntryController> _logger;
        private readonly ICollectionEntryService _paymentService;

        public AppCollectionEntryController(
             ILogger<AppCollectionEntryController> logger
            ,ICollectionEntryService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("GetCollectionList")]
        public async Task<IActionResult> GetCollectionList()
        {
            try
            {
                var result = await _paymentService.GetCollectionList();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetCollectionByType/{paymentFrom}")]
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _paymentService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PaymentModel model)
        {

            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (!await _paymentService.IsExistAsync(model.Id))
                {
                    ModelState.AddModelError(nameof(model.Name), "does not exist");
                    return ValidationResult(ModelState);
                }

                var result = await _paymentService.UpdateAsync(model);
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
                if (!await _paymentService.IsExistAsync(id))
                {
                    ModelState.AddModelError(nameof(id), "does not exist");
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


    }
}
