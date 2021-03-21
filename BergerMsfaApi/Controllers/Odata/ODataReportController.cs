using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Interfaces;
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

        public ODataReportController(IReportDataService reportDataService, IAuthService authService)
        {
            _reportDataService = reportDataService;
            _authService = authService;
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
    }
}
