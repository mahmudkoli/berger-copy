using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.OData.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ODataReportController : BaseController
    {
        private readonly IReportDataService _reportDataService;
        private readonly IAuthService _authService;
        private readonly IODataReportService _oDataReportService;

        public ODataReportController(IReportDataService reportDataService, IAuthService authService,IODataReportService oDataReportService)
        {
            _reportDataService = reportDataService;
            _authService = authService;
            _oDataReportService = oDataReportService;
        }

        [HttpGet("MyTargetReport")]
        public async Task<IActionResult> TerritoryWiseMyTarget([FromQuery] MyTargetSearchModel model)
        {
            try
            {
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                //dealerIds = new List<int>
                //{
                //    24,48,1852,1861,1835,1826,1796,1692,1681,1677,1610,4,8
                //};
                var result = await _reportDataService.MyTarget(model,dealerIds);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MySummaryReport")]
        public async Task<IActionResult> MySummaryReport()
        {
            try
            {
                var result = await _oDataReportService.MySummaryReport();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

         
    }
}
