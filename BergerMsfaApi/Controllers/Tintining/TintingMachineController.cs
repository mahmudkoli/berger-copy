using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Tintining.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Tintining
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class TintingMachineController : BaseController
    {
        private readonly ILogger<TintingMachineController> _logger;
        private readonly ITintiningService _tintiningService;
        public TintingMachineController(
            ITintiningService tintiningService,
            ILogger<TintingMachineController> logger
            )
        {
            _tintiningService = tintiningService;
            _logger = logger;
        }
        [HttpGet("GetTintingMachineList/{territory}")]
        public async Task<IActionResult> GetTintingMachineList(string territory)
        {
            try
            {
                var result = await _tintiningService.GetTintingMachineList(territory);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }

        }

    }
}
