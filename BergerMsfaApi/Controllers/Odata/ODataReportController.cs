using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.OData.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Odata
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppSalesReportController : BaseController
    {
        private readonly IReportDataService _reportDataService;
        private readonly IAuthService _authService;
        private readonly IODataReportService _oDataReportService;
        private readonly ISalesDataService _salesDataService;
        private readonly IFinancialDataService _financialDataService;
        private readonly IStockDataService _stockDataService;
        private readonly IBalanceDataService _balanceDataService;

        public AppSalesReportController(
            IReportDataService reportDataService, 
            IAuthService authService,
            IODataReportService oDataReportService,
            ISalesDataService salesDataService,
            IFinancialDataService financialDataService,
            IStockDataService stockDataService,
            IBalanceDataService balanceDataService)
        {
            _reportDataService = reportDataService;
            _authService = authService;
            _oDataReportService = oDataReportService;
            _salesDataService = salesDataService;
            _financialDataService = financialDataService;
            _stockDataService = stockDataService;
            _balanceDataService = balanceDataService;
        }

        [HttpGet("TodaysActivitySummary")]
        [ProducesResponseType(typeof(MySummaryReportResultModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTodaysActivitySummary()
        {
            try
            {
                var area = _authService.GetLoggedInUserArea();
                var result = await _oDataReportService.MySummaryReport(area);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("TodaysInvoiceValue")]
        [ProducesResponseType(typeof(IList<TotalInvoiceValueResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTodaysInvoiceValue([FromQuery] TotalInvoiceValueSearchModel model)
        {
            try
            {
                var area = _authService.GetLoggedInUserArea();
                var result = await _salesDataService.GetTotalInvoiceValue(model, area);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MTDTargetSummary")]
        [ProducesResponseType(typeof(IList<MTDTargetSummaryReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMTDTargetSummary([FromQuery] MTDTargetSummarySearchModel model)
        {
            try
            {
                var result = await _reportDataService.MTDTargetSummary(model);
                var employeeRole = AppIdentity.AppUser.EmployeeRole;
                if (employeeRole == (int)EnumEmployeeRole.BM_BSI || employeeRole == (int)EnumEmployeeRole.TM_TO || employeeRole == (int)EnumEmployeeRole.ZO)
                {
                    foreach (var item in result)
                    {
                        item.Depot = null;
                    }
                }
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MTDBrandPerformance")]
        [ProducesResponseType(typeof(IList<MTDBrandPerformanceReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMTDBrandPerformance([FromQuery] MTDBrandPerformanceSearchModel model)
        {
            try
            {
                var result = await _reportDataService.MTDBrandPerformance(model);
                var employeeRole = AppIdentity.AppUser.EmployeeRole;
                if (employeeRole == (int)EnumEmployeeRole.BM_BSI || employeeRole == (int)EnumEmployeeRole.TM_TO || employeeRole == (int)EnumEmployeeRole.ZO)
                {
                    foreach (var item in result)
                    {
                        item.Depot = null;
                    }
                }
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("BrandOrDivisionWisePerformance")]
        public async Task<IActionResult> GetBrandOrDivisionWisePerformance([FromQuery] BrandOrDivisionWisePerformanceSearchModel model)
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _salesDataService.GetReportBrandOrDivisionWisePerformance(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DealerPerformance")]
        public async Task<IActionResult> GetDealerPerformance([FromQuery] DealerPerformanceSearchModel model)
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _salesDataService.GetReportDealerPerformance(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("ReportDealerPerformance")]
        public async Task<IActionResult> ReportDealerPerformance([FromQuery] DealerPerformanceResultSearchModel model)
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _oDataReportService.ReportDealerPerformance(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingSummary")]
        public async Task<IActionResult> GetReportOutstandingSummary()
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _financialDataService.GetReportOutstandingSummary(dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OSOver90Days")]
        public async Task<IActionResult> GetReportOSOver90Days([FromQuery] OSOver90DaysSearchModel model)
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _financialDataService.GetReportOSOver90Days(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("PaymentFollowUp")]
        public async Task<IActionResult> GetReportPaymentFollowUp([FromQuery] PaymentFollowUpSearchModel model)
        {
            try
            {
                IList<string> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _financialDataService.GetReportPaymentFollowUp(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("StockDetails")]
        public async Task<IActionResult> GetStockDetails([FromQuery] StocksSearchModel model)
        {
            try
            {
                var result = await _stockDataService.GetStockDetails(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("CustomerCredit")]
        public async Task<IActionResult> GetCustomerCredit([FromQuery] CustomerCreditSearchModel model)
        {
            try
            {
                var result = await _balanceDataService.GetCustomerCredit(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
