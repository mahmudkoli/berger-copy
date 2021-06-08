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
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("InvoiceDetails")]
        public async Task<IActionResult> GetInvoiceDetails([FromQuery] InvoiceDetailsSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetInvoiceDetails(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("BrandWiseMTDDetails")]
        public async Task<IActionResult> GetBrandWiseMTDDetails([FromQuery] BrandWiseMTDSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetBrandWiseMTDDetails(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("MTSBrandsVolume")]
        public async Task<IActionResult> GetMTSBrandsVolume([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsDataService.GetMTSBrandsVolume(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("PremiumBrandPerformance")]
        public async Task<IActionResult> GetPremiumBrandPerformance([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsDataService.GetPremiumBrandPerformance(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("MonthlyValueTarget")]
        public async Task<IActionResult> GetMonthlyValueTarget([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsDataService.GetMonthlyValueTarget(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("BrandOrDivisionWisePerformance")]
        public async Task<IActionResult> GetBrandOrDivisionWisePerformance([FromQuery] BrandOrDivisionWiseMTDSearchModel model)
        {
            try
            {
                var data = await _salesDataService.GetBrandOrDivisionWisePerformance(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
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
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingSummary")]
        public async Task<IActionResult> GetOutstandingSummary([FromQuery] OutstandingSummarySearchModel model)
        {
            try
            {
                var data = await _financialDataService.GetOutstandingSummary(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }
        #endregion

        #region Balance Collection Data
        [HttpGet("CollectionHistory")]
        public async Task<IActionResult> GetCollectionHistory([FromQuery] CollectionHistorySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetCollectionHistory(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("BalanceConfirmationSummary")]
        public async Task<IActionResult> GetBalanceConfirmationSummary([FromQuery] BalanceConfirmationSummarySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetBalanceConfirmationSummary(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("ChequeBounce")]
        public async Task<IActionResult> GetChequeBounce([FromQuery] ChequeBounceSearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetChequeBounce(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("ChequeSummary")]
        public async Task<IActionResult> GetChequeSummary([FromQuery] ChequeSummarySearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetChequeSummary(model);
                return AppOkResult(data);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }
        #endregion
    }
}
