using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
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
        public AppKpiReportController(IKpiDataService kpiDataService, IKPIReportService kpiReportService)
        {
            _kpiDataService = kpiDataService;
            _kpiReportService = kpiReportService;
        }

        [HttpGet("GetBusinessCallAnalysis")]
        [ProducesResponseType(typeof(IList<BusinessCallAPPKPIReportResultModel>), (int)HttpStatusCode.OK)]
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

        [HttpGet("GetPremiumBrandBillingStrikeRate")]
        [ProducesResponseType(typeof(IList<StrikeRateKPIReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPremiumBrandBillingStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.PremiumBrandBillingStrikeRateKPIReportAsync(model,EnumReportFor.App);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetBillingAnalysis")]
        public async Task<IActionResult> GetBillingAnalysisKPIReport([FromQuery] BillingAnalysisKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBillingAnalysisKPIReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetColorBankInstallationPlanVsActual")]
        [ProducesResponseType(typeof(IList<ColorBankInstallationPlanVsActualKPIReportResultModel>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetColorBankInstallationPlanVsActual([FromQuery] ColorBankInstallationPlanVsActualKpiReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetColorBankInstallationPlanVsActual(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        //[HttpGet("GetColorBankProductivity")]
        //[ProducesResponseType(typeof(IList<ColorBankInstallationPlanVsActualKPIReportResultModel>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetColorBankProductivity([FromQuery] ColorBankInstallationPlanVsActualKpiReportSearchModel model)
        //{
        //    try
        //    {
        //        var result = _kpiReportService.GetColorBankInstallationPlanVsActual(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

    }
}
