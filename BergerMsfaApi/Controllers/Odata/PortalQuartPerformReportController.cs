using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class PortalQuartPerformReportController : BaseController
    {
        private readonly IQuarterlyPerformanceDataService _quarterlyPerformanceDataService;
        private readonly IFinancialDataService _financialDataService;

        public PortalQuartPerformReportController(
            IQuarterlyPerformanceDataService quarterlyPerformanceDataService, 
            IFinancialDataService financialDataService)
        {
            _quarterlyPerformanceDataService = quarterlyPerformanceDataService;
            _financialDataService = financialDataService;
        }

        [HttpGet("GetMTSValueTargetAchivement")]
        public async Task<IActionResult> GetMTSValueTargetAchivement([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var data = await _quarterlyPerformanceDataService.GetMTSValueTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadMTSValueTargetAchivement")]
        public async Task<IActionResult> DownloadMTSValueTargetAchivement([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var data = await _quarterlyPerformanceDataService.GetMTSValueTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetBillingDealerQuarterlyGrowth")]
        public async Task<IActionResult> GetBillingDealerQuarterlyGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var data = await _quarterlyPerformanceDataService.GetBillingDealerQuarterlyGrowth(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadBillingDealerQuarterlyGrowth")]
        public async Task<IActionResult> DownloadBillingDealerQuarterlyGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var data = await _quarterlyPerformanceDataService.GetBillingDealerQuarterlyGrowth(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetEnamelPaintsQuarterlyGrowth")]
        public async Task<IActionResult> GetEnamelPaintsQuarterlyGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetEnamelPaintsQuarterlyGrowth(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadEnamelPaintsQuarterlyGrowth")]
        public async Task<IActionResult> DownloadEnamelPaintsQuarterlyGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetEnamelPaintsQuarterlyGrowth(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPremiumBrandsGrowth")]
        public async Task<IActionResult> GetPremiumBrandsGrowthGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetPremiumBrandsGrowth(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPremiumBrandsGrowth")]
        public async Task<IActionResult> DownloadPremiumBrandsGrowthGrowth([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetPremiumBrandsGrowth(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet("GetPremiumBrandsContribution")]
        public async Task<IActionResult> GetPremiumBrandsContribution([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetPremiumBrandsContribution(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpGet("DownloadPremiumBrandsContribution")]
        public async Task<IActionResult> DownloadPremiumBrandsContribution([FromQuery] PortalQuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result= await _quarterlyPerformanceDataService.GetPremiumBrandsContribution(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Os over 90 days Trend Report
        [HttpGet("OsOver90DaysTrendReport")]
        public async Task<IActionResult> OsOver90DaysTrendReport([FromQuery] PortalOSOver90DaysTrendSearchModel query)
        {
            try
            {
                var result = await _financialDataService.GetPortalOSOver90DaysTrendReport(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadOsOver90daysTrendReport")]
        public async Task<IActionResult> DownloadOsOver90daysTrendReport([FromQuery] PortalOSOver90DaysTrendSearchModel query)
        {
            try
            {
                var result = await _financialDataService.GetPortalOSOver90DaysTrendReport(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}
