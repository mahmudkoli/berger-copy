using BergerMsfaApi.Models.Report;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BergerMsfaApi.Services.Report.Interfaces;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Controllers.Common;

namespace BergerMsfaApi.Controllers.Odata
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppKpiReportController : BaseController
    {
        private readonly IKPIReportService _kpiReportService;

        public AppKpiReportController(IKPIReportService kpiReportService)
        {
            _kpiReportService = kpiReportService;
        }

        [HttpGet("GetFinancialCollectionPlan")]
        public async Task<IActionResult> GetFinancialCollectionPlanKPIReport([FromQuery] CollectionPlanKPIReportSearchModelForApp model)
        {
            try
            {
                var result = await _kpiReportService.GetFinancialCollectionPlanKPIReportForAppAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
