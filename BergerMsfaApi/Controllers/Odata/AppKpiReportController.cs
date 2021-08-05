using System;
using System.Threading.Tasks;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Odata
{
    //[AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppKpiReportController : BaseController
    {
        private readonly IKpiDataService _kpiDataService;
        private readonly IKPIReportService _kpiReportService;
        public AppKpiReportController(IKpiDataService kpiDataService,
            IKPIReportService kpiReportService)
        {
            _kpiDataService = kpiDataService;
            _kpiReportService = kpiReportService;
        }

        [HttpGet("GetBusinessCallAnalysis")]
        public async Task<IActionResult> GetBusinessCallKpiReport([FromQuery] BusinessCallKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model,EnumReportFor.App);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


    }
}
