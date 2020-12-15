using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DealerOpeningController : BaseController
    {
        private ILogger<DealerOpeningController> _logger;
        private readonly IDealerOpeningService _dealerOpeningSvc;
        public DealerOpeningController(ILogger<DealerOpeningController> logger
            , IDealerOpeningService dealerOpeningSvc)
        {
            _logger = logger;
            _dealerOpeningSvc = dealerOpeningSvc;
        }


        [HttpGet("GetDealerOpeningList/{index}/{pageSize}")]
        public async Task<IActionResult> GetDealerOpeningListAsync(int index, int pageSize, string search)
        {
            try
            {
                var result = await _dealerOpeningSvc.GetDealerOpeningListAsync(index, pageSize, search);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

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
