using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Scheme
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppSchemeDetailController : BaseController
    {

        private readonly ISchemeService _schemeService;

        public AppSchemeDetailController(
            ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }

        [HttpGet("GetSchemeDetailList")]
        public async Task<IActionResult> GetSchemeDetailList()
        {
            try
            {
                var result = await _schemeService.GetAppAllSchemeDetailsByCurrentUserAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
