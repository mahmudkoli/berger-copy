using System;
using System.Net;
using System.Threading.Tasks;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace BergerMsfaApi.Controllers.Common
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboard;
        public DashboardController(ILogger<DashboardController> logger,
            IDashboardService dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }


        /// <summary>
        /// Return a list of POSMProduct Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        [SwaggerOperation("GetAllDashboardData")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> GetAllDashboardData()
        {
            try
            {
       

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}