using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        public AppCollectionEntryController( ILogger<AppCollectionEntryController> logger
            ,ICollectionEntryService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }
        
        [HttpGet("[action]")]
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

        [HttpGet("[action]/{paymentFrom}")]
        public async Task<IActionResult> GetCollectionByType(string paymentFrom)
        {

            try
            {
                if (string.IsNullOrEmpty(paymentFrom)) return ValidationResult(null);
                var result = await _paymentService.GetCollectionByType(paymentFrom);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

  


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] PaymentModel model)
        {

            try
            {

                var result = await _paymentService.CreateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] PaymentModel model)
        {

            try
            {
                var isExist = await _paymentService.IsExistAsync(model);
                if (isExist)
                {
                    var result = await _paymentService.UpdateAsync(model);
                    return OkResult(result);

                }
                else
                {
                    ModelState.AddModelError(nameof(model.Id), "Does Not Exist");
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
