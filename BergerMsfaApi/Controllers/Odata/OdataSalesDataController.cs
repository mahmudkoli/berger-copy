using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.OData.Interfaces;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OdataSalesDataController : BaseController
    {
        private readonly ISalesDataService _salesDataService;
        private readonly IMTSDataService _mtsDataService;
        private readonly IFinancialDataService _financialDataService;
        private readonly IBalanceDataService _balanceDataService;

        public OdataSalesDataController(
            ISalesDataService salesDataService,
            IMTSDataService mtsDataService,
            IFinancialDataService financialDataService,
            IBalanceDataService balanceDataService
            )
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

        [HttpGet("BrandWiseMTDDetails")]
        public async Task<IActionResult> GetBrandWiseMTDDetails([FromQuery] BrandWiseMTDSearchModel model)
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

        [HttpGet("MTSBrandsVolume")]
        public async Task<IActionResult> GetMTSBrandsVolume([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsDataService.GetMTSBrandsVolume(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("PremiumBrandPerformance")]
        public async Task<IActionResult> GetPremiumBrandPerformance([FromQuery] MTSSearchModel model)
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

        [HttpGet("MonthlyValueTarget")]
        public async Task<IActionResult> GetMonthlyValueTarget([FromQuery] MTSSearchModel model)
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

        [HttpGet("BrandOrDivisionWisePerformance")]
        public async Task<IActionResult> GetBrandOrDivisionWisePerformance([FromQuery] BrandOrDivisionWiseMTDSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetBrandOrDivisionWisePerformance(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion

        #region Financial Data
        [HttpGet("CollectionHistory")]
        public async Task<IActionResult> GetCollectionHistory([FromQuery] CollectionHistorySearchModel model)
        {
            try
            {
                var data = await _financialDataService.GetCollectionHistory(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

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

        #region Balance Data
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

        [HttpGet("ChequeBounce")]
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

        [HttpGet("ChequeSummary")]
        public async Task<IActionResult> GetChequeSummary([FromQuery] ChequeSummarySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetChequeSummary(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion
    }
}
