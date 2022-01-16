using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DealerOpeningController : BaseController
    {
        private ILogger<DealerOpeningController> _logger;
        private readonly IDealerOpeningService _dealerOpeningSvc;

        public DealerOpeningController(
            ILogger<DealerOpeningController> logger, 
            IDealerOpeningService dealerOpeningSvc)
        {
            _logger = logger;
            _dealerOpeningSvc = dealerOpeningSvc;
        }

        [HttpGet("GetDealerOpeningList")]
        public async Task<IActionResult> GetDealerOpeningListAsync([FromQuery] DealerOpeningQueryObjectModel query)
        {
            try
            {
                var result = await _dealerOpeningSvc.GetAllDealersAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("ChangeDealerOpeningStatus")]
        public async Task<IActionResult> ChangePlanStatus(DealerOpeningStatusChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _dealerOpeningSvc.ChangeStatusDealer(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            };
        }

        [HttpGet("GetDealerOpeningDetailById/{id}")]
        public async Task<IActionResult> GetDealerOpeningDetailById(int id)
        {
            try
            {
                var result = await _dealerOpeningSvc.GetDealerOpeningDetailById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
