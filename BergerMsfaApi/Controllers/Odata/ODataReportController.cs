using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.OData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        private readonly IRepository<DealerInfo> _dealerInfoRepository;

        public AppSalesReportController(
            IReportDataService reportDataService,
            IAuthService authService,
            IODataReportService oDataReportService,
            ISalesDataService salesDataService,
            IFinancialDataService financialDataService,
            IStockDataService stockDataService,
            IBalanceDataService balanceDataService,
            IRepository<DealerInfo> dealerInfoRepository

            )
        {
            _reportDataService = reportDataService;
            _authService = authService;
            _oDataReportService = oDataReportService;
            _salesDataService = salesDataService;
            _financialDataService = financialDataService;
            _stockDataService = stockDataService;
            _balanceDataService = balanceDataService;
            _dealerInfoRepository = dealerInfoRepository;
        }

        [HttpGet("TodaysActivitySummary")]
        [ProducesResponseType(typeof(TodaysActivitySummaryReportResultModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTodaysActivitySummary()
        {
            try
            {
                var area = _authService.GetLoggedInUserArea();
                var result = await _oDataReportService.TodaysActivitySummaryReport(area);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("TodaysInvoiceValue")]
        [ProducesResponseType(typeof(IList<TodaysInvoiceValueResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTodaysInvoiceValue([FromQuery] TodaysInvoiceValueSearchModel model)
        {
            try
            {
                var area = _authService.GetLoggedInUserArea();
                var result = await _salesDataService.GetTodaysActivityInvoiceValue(model, area);
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

        [HttpGet("YTDBrandPerformance")]
        [ProducesResponseType(typeof(IList<YTDBrandPerformanceSearchModelResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBrandOrDivisionWisePerformance([FromQuery] YTDBrandPerformanceSearchModelSearchModel model)
        {
            try
            {
                var result = await _salesDataService.GetYTDBrandPerformance(model);
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

        [HttpGet("CategoryWiseDealerPerformance")]
        [ProducesResponseType(typeof(IList<CategoryWiseDealerPerformanceResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDealerPerformance([FromQuery] CategoryWiseDealerPerformanceSearchModel model)
        {
            try
            {
                var result = await _salesDataService.GetCategoryWiseDealerPerformance(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingSummary")]
        [ProducesResponseType(typeof(IList<OutstandingSummaryReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReportOutstandingSummary([FromQuery] OutstandingSummaryReportSearchModel model)
        {
            try
            {
                var result = await _financialDataService.GetOutstandingSummaryReport(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OSOver90Days")]
        [ProducesResponseType(typeof(IList<OSOver90DaysReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReportOSOver90Days([FromQuery] OSOver90DaysSearchModel model)
        {
            try
            {
                var result = await _financialDataService.GetOSOver90DaysReport(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("ChequeSummary")]
        [ProducesResponseType(typeof(ChequeSummaryReportResultModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetChequeSummary([FromQuery] ChequeSummaryReportSearchModel model)
        {
            try
            {
                var data = await _balanceDataService.GetChequeSummaryReport(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MaterialStock")]
        [ProducesResponseType(typeof(IList<MaterialStockResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMaterialStock([FromQuery] MaterialStockSearchModel model)
        {
            try
            {
                var result = await _stockDataService.GetMaterialStock(model);
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



        [HttpGet("ReportLastYearAppointedDealerPerformanceSummary")]
        public async Task<IActionResult> ReportLastYearAppointedDealerPerformanceSummary([FromQuery] LastYearAppointedDealerPerformanceSearchModel model)
        {
            try
            {
                var result = await _oDataReportService.ReportLastYearAppointedDealerPerformanceSummary(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("ReportLastYearAppointedDealerPerformanceDetail")]
        public async Task<IActionResult> ReportLastYearAppointedDealerPerformanceDetail([FromQuery] LastYearAppointedDealerPerformanceSearchModel model)
        {
            try
            {
                var result = await _oDataReportService.ReportLastYearAppointedDealerPerformanceDetails(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("ReportRprsFollowup")]
        public async Task<IActionResult> ReportRprsFollowup([FromQuery] RprsFollowupSearchModel model)
        {
            try
            {

                var result = await _financialDataService.GetPaymentFollowUp(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


    }
}
