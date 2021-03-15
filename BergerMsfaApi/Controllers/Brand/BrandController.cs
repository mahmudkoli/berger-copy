using System;
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
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObjectModel query)
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

        [HttpPost("UpdateBrandStatus")]
        public async Task<IActionResult> BrandStatusUpdate([FromBody] BrandStatusModel brandStatus)
        {
            try
            {
                var result = await _brandService.BrandStatusUpdate(brandStatus);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetBrandInfoStatusLog/{id}")]
        public async Task<IActionResult> GetBrandInfoStatusLogDetails(int id)
        {
            try
            {
                var result = await _brandService.GetBrandInfoStatusLog(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
    }
}
