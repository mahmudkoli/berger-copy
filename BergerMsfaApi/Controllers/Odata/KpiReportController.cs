using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Odata
{
  //  [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class KpiReportController : BaseController
    {
        private readonly IKpiDataService _kpiDataService;
        private readonly IKPIReportService _kpiReportService;
        private readonly IUniverseReachAnalysisService _universeReachAnalysisService;

        public KpiReportController(
            IKpiDataService kpiDataService,
            IKPIReportService kpiReportService,
            IUniverseReachAnalysisService universeReachAnalysisService)
        {
            _kpiDataService = kpiDataService;
            _kpiReportService = kpiReportService;
            _universeReachAnalysisService = universeReachAnalysisService;
        }

        [HttpGet("GetTerritoryTargetAchivement")]
        public async Task<IActionResult> GetTerritoryTargetAchivement([FromQuery] SalesTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetTerritoryTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadTerritoryTargetAchivement")]
        public async Task<IActionResult> DownloadTerritoryTargetAchivement([FromQuery] SalesTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetTerritoryTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerWiseTargetAchivement")]
        public async Task<IActionResult> GetDealerWiseTargetAchivement([FromQuery] DealerWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetDealerWiseTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerWiseTargetAchivement")]
        public async Task<IActionResult> DownloadDealerWiseTargetAchivement([FromQuery] DealerWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetDealerWiseTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetProductWiseTargetAchivement")]
        public async Task<IActionResult> GetProductWiseTargetAchivement([FromQuery] ProductWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetProductWiseTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadProductWiseTargetAchivement")]
        public async Task<IActionResult> DownloadProductWiseTargetAchivement([FromQuery] ProductWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetProductWiseTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetBusinessCallAnalysis")]
        public async Task<IActionResult> GetBusinessCallKPIReport([FromQuery] BusinessCallKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model, EnumReportFor.Web);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadBusinessCallAnalysis")]
        public async Task<IActionResult> DownloadBusinessCallKPIReport([FromQuery] BusinessCallKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model, EnumReportFor.Web);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPremiumBrandBillingStrikeRate")]
        public async Task<IActionResult> GetPremiumBrandBillingStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.PremiumBrandBillingStrikeRateKPIReportAsync(model,EnumReportFor.Web);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPremiumBrandBillingStrikeRate")]
        public async Task<IActionResult> DownloadStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.PremiumBrandBillingStrikeRateKPIReportAsync(model,EnumReportFor.Web);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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

        [HttpGet("DownloadBillingAnalysis")]
        public async Task<IActionResult> DownloadBillingAnalysisKPIReport([FromQuery] BillingAnalysisKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBillingAnalysisKPIReportAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetUniverseReachAnalysis")]
        public async Task<IActionResult> GetUniverseReachAnalysisReportReport([FromQuery] UniverseReachAnalysisReportSearchModel model)
        {
            try
            {
                var result = await _universeReachAnalysisService.GetUniverseReachAnalysisReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadUniverseReachAnalysis")]
        public async Task<IActionResult> DownloadUniverseReachAnalysisReportReport([FromQuery] UniverseReachAnalysisReportSearchModel model)
        {
            try
            {
                var result = await _universeReachAnalysisService.GetUniverseReachAnalysisReportAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetFinancialCollectionPlan")]
        public async Task<IActionResult> GetFinancialCollectionPlanKPIReport([FromQuery] CollectionPlanKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetFinancialCollectionPlanKPIReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadFinancialCollectionPlan")]
        public async Task<IActionResult> DownloadFinancialCollectionPlanKPIReport([FromQuery] CollectionPlanKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetFinancialCollectionPlanKPIReportAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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

        [HttpGet("GetColorBankProductivity")]
        [ProducesResponseType(typeof(IList<ColorBankProductivityBase>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetColorBankProductivity([FromQuery] ColorBankProductivityKpiReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetColorBankProductivity(model, EnumReportFor.Web);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}
