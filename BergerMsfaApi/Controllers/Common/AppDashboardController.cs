using System;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Common
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboard;

        public AppDashboardController(
            ILogger<DashboardController> logger,
            IDashboardService dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppDashboardData()
        {
            try
            {
                var result = await _dashboard.GetAppDashboardDataAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}