﻿using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Brand;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Services.Brand.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppBrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public AppBrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] AppBrandSearchModel query)
        {
            try
            {
                var result = await _brandService.GetBrandsAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
