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
    public class AppDealerVisitReportController : BaseController
    {
        private readonly ISalesDataService _salesDataService;
        private readonly IMTSDataService _mtsDataService;
        private readonly IFinancialDataService _financialDataService;
        private readonly IBalanceDataService _balanceDataService;

        public AppDealerVisitReportController(
            ISalesDataService salesDataService,
            IMTSDataService mtsDataService,
            IFinancialDataService financialDataService,
            IBalanceDataService balanceDataService)
        {
            _salesDataService = salesDataService;
            _mtsDataService = mtsDataService;
            _financialDataService = financialDataService;
            _balanceDataService = balanceDataService;
        }

        #region Sales Data
        [HttpGet("InvoiceHistory")]
        public async Task<IActionResult> GetInvoiceHistory([FromQuery] InvoiceHistorySearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetInvoiceHistory(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("InvoiceDetails")]
        public async Task<IActionResult> GetInvoiceDetails([FromQuery] InvoiceDetailsSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetInvoiceDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("BrandWiseLiftingTrend")]
        public async Task<IActionResult> GetBrandWiseLiftingTrend([FromQuery] BrandWiseMTDSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetBrandWiseMTDDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MTSUpdate")]
        public async Task<IActionResult> GetMTSUpdate([FromQuery] MTSSearchModelBase model)
        {
            try
            {
                var data = await _mtsDataService.GetMTSUpdate(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("PremiumBrandTargetUpdate")]
        public async Task<IActionResult> GetPremiumBrandTargetUpdate([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsDataService.GetPremiumBrandPerformance(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MonthlyValueTargetUpdate")]
        public async Task<IActionResult> GetMonthlyValueTarget([FromQuery] MTSSearchModelBase model)
        {
            try
            {
                var data = await _mtsDataService.GetMonthlyValueTarget(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("BrandWisePerformance")]
        public async Task<IActionResult> GetBrandWisePerformance([FromQuery] BrandWisePerformanceSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetBrandWisePerformance(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion

        #region Financial Data
        [HttpGet("OutstandingDetails")]
        public async Task<IActionResult> GetOutstandingDetails([FromQuery] OutstandingDetailsSearchModel model)
        {
            try
            {
                var data = await _financialDataService.GetOutstandingDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingSummary")]
        public async Task<IActionResult> GetOutstandingSummary([FromQuery] OutstandingSummarySearchModel model)
        {
            try
            {
                var data = await _financialDataService.GetOutstandingSummary(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion

        #region Balance Collection Data
        [HttpGet("MRHistory")]
        public async Task<IActionResult> GetMrHistory([FromQuery] CollectionHistorySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetMRHistory(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("BalanceConfirmationSummary")]
        public async Task<IActionResult> GetBalanceConfirmationSummary([FromQuery] BalanceConfirmationSummarySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetBalanceConfirmationSummary(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("ChequeSummary")]
        public async Task<IActionResult> GetChequeBounce([FromQuery] ChequeBounceSearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetChequeBounce(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpGet("ChequeSummary")]
        //public async Task<IActionResult> GetChequeSummary([FromQuery] ChequeSummarySearchModel model)
        //{
        //    try
        //    {
        //        var data = await _balanceDataService.GetChequeSummary(model);
        //        return OkResult(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}
        #endregion
    }
}
