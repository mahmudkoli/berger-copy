using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerSalesCall
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DealerSalesCallController : BaseController
    {
        private readonly IDealerSalesCallService _dealerSalesCallService;

        public DealerSalesCallController(
                IDealerSalesCallService dealerSalesCallService
            )
        {
            this._dealerSalesCallService = dealerSalesCallService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DealerSalesCallQueryObjectModel query)
        {
            try
            {
                var result = await _dealerSalesCallService.GetAllAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _dealerSalesCallService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
