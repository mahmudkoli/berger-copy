﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Tinting.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Tinting
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

        [HttpGet("GetTintingMachinePagingList")]
        public async Task<IActionResult> GetTintingMachinePagingList(int index, int pageSize, string search)
        {
            try
            {
                var result = await _tintiningService.GetTintingMachinePagingList(index, pageSize, search);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

    }
}