﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.OData.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ODataReportController : BaseController
    {
        private readonly IReportDataService _reportDataService;
        private readonly IAuthService _authService;
        private readonly IODataReportService _oDataReportService;
        private readonly ISalesDataService _salesDataService;
        private readonly IFinancialDataService _financialDataService;

        public ODataReportController(
            IReportDataService reportDataService, 
            IAuthService authService,
            IODataReportService oDataReportService,
            ISalesDataService salesDataService,
            IFinancialDataService financialDataService)
        {
            _reportDataService = reportDataService;
            _authService = authService;
            _oDataReportService = oDataReportService;
            _salesDataService = salesDataService;
            _financialDataService = financialDataService;
        }

        [HttpGet("MyTargetReport")]
        public async Task<IActionResult> TerritoryWiseMyTarget([FromQuery] MyTargetSearchModel model)
        {
            try
            {
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                //dealerIds = new List<int>
                //{
                //    24,48,1852,1861,1835,1826,1796,1692,1681,1677,1610,4,8
                //};
                var result = await _reportDataService.MyTarget(model, dealerIds);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MySummaryReport")]
        public async Task<IActionResult> MySummaryReport()
        {
            try
            {
                var result = await _oDataReportService.MySummaryReport();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("TotalInvoiceValue")]
        public async Task<IActionResult> GetTotalInvoiceValue([FromQuery] TotalInvoiceValueSearchModel model)
        {
            try
            {
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _salesDataService.GetReportTotalInvoiceValue(model, dealerIds);
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
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
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
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
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
                var result = await _oDataReportService.ReportDealerPerformance(model);
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
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
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
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
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
                IList<int> dealerIds = await _authService.GetDealerByUserId(AppIdentity.AppUser.UserId);
                var result = await _financialDataService.GetReportPaymentFollowUp(model, dealerIds);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
