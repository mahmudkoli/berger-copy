using BergerMsfaApi.Models.Report;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BergerMsfaApi.Services.Report.Interfaces;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Models.KPI;
using System.Collections.Generic;

namespace BergerMsfaApi.Controllers.Odata
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppKpiReportController : BaseController
    {
        private readonly IKPIReportService _kpiReportService;
        private readonly INewDealerDevelopmentService _newDealerDevelopmentService;


        public AppKpiReportController(IKPIReportService kpiReportService, INewDealerDevelopmentService newDealerDevelopmentService)
        {
            _kpiReportService = kpiReportService;
            _newDealerDevelopmentService = newDealerDevelopmentService;
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



        [HttpGet("GetDealerConversionData")]
        public async Task<IActionResult> GetDealerConversionData([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetDealerConversionByYearAsync(model);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("SaveDealerConversion")]
        public async Task<IActionResult> SaveDealerConversion(IList<NewDealerDevelopmentSaveModel> model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.AddDealerConversionAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerOpeningStatusReport")]
        public async Task<IActionResult> GetDealerOpeningStatusReport([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetNewDealerDevelopment(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetDealerConversionReport")]
        public async Task<IActionResult> GetDealerConversionReport([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetDealerConversion(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
