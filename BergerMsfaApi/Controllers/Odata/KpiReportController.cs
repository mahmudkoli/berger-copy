using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class KpiReportController : BaseController
    {
        private readonly IKpiDataService _kpiDataService;
        private readonly IKPIReportService _kpiReportService;

        public KpiReportController(
            IKpiDataService kpiDataService,
            IKPIReportService kpiReportService
            )
        {
            _kpiDataService = kpiDataService;
            this._kpiReportService = kpiReportService;
        }

        [HttpGet("GetTerritoryTargetAchivement")]
        public async Task<IActionResult> GetTerritoryTargetAchivement([FromQuery] TerritoryTargetAchievementSearchModel model)
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
        public async Task<IActionResult> DownloadTerritoryTargetAchivement([FromQuery] TerritoryTargetAchievementSearchModel model)
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
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model);
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
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetStrikeRateOnBusinessCall")]
        public async Task<IActionResult> GetStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetStrikeRateKPIReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadStrikeRateOnBusinessCall")]
        public async Task<IActionResult> DownloadStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetStrikeRateKPIReportAsync(model);
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
    }
}
