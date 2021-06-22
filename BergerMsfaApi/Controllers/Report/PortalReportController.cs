﻿using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Filters;

namespace BergerMsfaApi.Controllers.Report
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class PortalReportController : BaseController
    {
        private readonly IPortalReportService _portalReportService;
        private readonly ICommonService _commonService;

        public PortalReportController(
                IPortalReportService portalReportService,
                ICommonService commonService)
        {
            _portalReportService = portalReportService;
            _commonService = commonService;
        }

        [HttpGet("GetLeadSummary")]
        public async Task<IActionResult> GetLeadSummary([FromQuery] LeadSummaryReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadSummaryReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadSummary")]
        public async Task<IActionResult> DownloadLeadSummary([FromQuery] LeadSummaryReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadSummaryReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLeadGenerationDetails")]
        public async Task<IActionResult> GetLeadGenerationDetails([FromQuery] LeadGenerationDetailsReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadGenerationDetailsReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadGenerationDetails")]
        public async Task<IActionResult> DownloadLeadGenerationDetails([FromQuery] LeadGenerationDetailsReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadGenerationDetailsReportAsync(query);
                
                _commonService.SetEmptyString(result.Items.ToList(), nameof(LeadGenerationDetailsReportResultModel.ImageUrl));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLeadFollowUpDetails")]
        public async Task<IActionResult> GetLeadFollowUpDetails([FromQuery] LeadFollowUpDetailsReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadFollowUpDetailsReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadFollowUpDetails")]
        public async Task<IActionResult> DownloadLeadFollowUpDetails([FromQuery] LeadFollowUpDetailsReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadFollowUpDetailsReportAsync(query);
                
                _commonService.SetEmptyString(result.Items.ToList(), nameof(LeadFollowUpDetailsReportResultModel.ImageUrl));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Nasir
        [HttpGet("GetPainterRegistration")]
        public async Task<IActionResult> GetPainterRegistration([FromQuery] PainterRegistrationReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetPainterRegistrationReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPainterRegistration")]
        public async Task<IActionResult> DownloadPainterRegistration([FromQuery] PainterRegistrationReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetPainterRegistrationReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(), nameof(PainterRegistrationReportResultModel.PainterImageUrl));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerOpening")]
        public async Task<IActionResult> GetDealerOpening([FromQuery] DealerOpeningReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerOpeningReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerOpening")]
        public async Task<IActionResult> DownloadDealerOpening([FromQuery] DealerOpeningReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerOpeningReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(), 
                    nameof(DealerOpeningReportResultModel.DealershipOpeningApplicationForm),
                    nameof(DealerOpeningReportResultModel.NomineePhotograph),
                    nameof(DealerOpeningReportResultModel.PhotographOfproprietor),
                    nameof(DealerOpeningReportResultModel.Cheque),
                    nameof(DealerOpeningReportResultModel.TradeLicensee),
                    nameof(DealerOpeningReportResultModel.IdentificationNo),
                    nameof(DealerOpeningReportResultModel.NomineeIdentificationNo));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerCollection")]
        public async Task<IActionResult> GetDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerCollection")]
        public async Task<IActionResult> DownloadDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerCollection")]
        public async Task<IActionResult> GetSubDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSubDealerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerCollection")]
        public async Task<IActionResult> DownloadSubDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSubDealerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetCustomerCollection")]
        public async Task<IActionResult> GetCustomerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetCustomerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadCustomerCollection")]
        public async Task<IActionResult> DownloadCustomerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetCustomerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetProjectCollection")]
        public async Task<IActionResult> GetProjectCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDirectProjectCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadProjectCollection")]
        public async Task<IActionResult> DownloadProjectCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDirectProjectCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPaintersCall")]
        public async Task<IActionResult> GetPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetPainterCallReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPaintersCall")]
        public async Task<IActionResult> DownloadPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetPainterCallReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerVisit")]
        public async Task<IActionResult> GetDealerVisit([FromQuery] DealerVisitReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerVisitReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerVisit")]
        public async Task<IActionResult> DownloadDealerVisit([FromQuery] DealerVisitReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerVisitReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerSalesCall")]
        public async Task<IActionResult> GetDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerSalesCallReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerSalesCall")]
        public async Task<IActionResult> DownloadDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerSalesCallReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(), 
                    nameof(DealerSalesCallReportResultModel.ProductDisplayAndMerchendizingImage),
                    nameof(DealerSalesCallReportResultModel.SchemeModalityImage));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerSalesCall")]
        public async Task<IActionResult> GetSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSubDealerSalesCallReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerSalesCall")]
        public async Task<IActionResult> DownloadSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSubDealerSalesCallReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(),
                    nameof(SubDealerSalesCallReportResultModel.ProductDisplayAndMerchendizingImage),
                    nameof(SubDealerSalesCallReportResultModel.SchemeModalityImage));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocDealerSalesCall")]
        public async Task<IActionResult> GetAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetAddhocDealerSalesCallReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocDealerSalesCall")]
        public async Task<IActionResult> DownloadAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetAddhocDealerSalesCallReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(),
                    nameof(DealerSalesCallReportResultModel.ProductDisplayAndMerchendizingImage),
                    nameof(DealerSalesCallReportResultModel.SchemeModalityImage));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocSubDealerSalesCall")]
        public async Task<IActionResult> GetAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetAddhocSubDealerSalesCallReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocSubDealerSalesCall")]
        public async Task<IActionResult> DownloadAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetAddhocSubDealerSalesCallReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(),
                    nameof(SubDealerSalesCallReportResultModel.ProductDisplayAndMerchendizingImage),
                    nameof(SubDealerSalesCallReportResultModel.SchemeModalityImage));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerIssue")]
        public async Task<IActionResult> GetDealerIssue([FromQuery] DealerIssueReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerIssueReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerIssue")]
        public async Task<IActionResult> DownloadDealerIssue([FromQuery] DealerIssueReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerIssueReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerIssue")]
        public async Task<IActionResult> GetSubDealerIssue([FromQuery] SubDealerIssueReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSubDealerIssueReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerIssue")]
        public async Task<IActionResult> DownloadSubDealerIssue([FromQuery] SubDealerIssueReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSubDealerIssueReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetTintingMachine")]
        public async Task<IActionResult> GetTintingMachine([FromQuery] TintingMachineReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetTintingMachineReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadTintingMachine")]
        public async Task<IActionResult> DownloadTintingMachine([FromQuery] TintingMachineReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetTintingMachineReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetActiveSummery")]
        public async Task<IActionResult> GetActiveSummery([FromQuery] ActiveSummeryReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetActiveSummeryReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadActiveSummery")]
        public async Task<IActionResult> DownloadActiveSummery([FromQuery] ActiveSummeryReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetActiveSummeryReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLogInReport")]
        public async Task<IActionResult> GetLogInReport([FromQuery] LogInReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLogInReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLogInReport")]
        public async Task<IActionResult> DownloadLogInReport([FromQuery] LogInReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLogInReportAsync(query);

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSnapShotReport")]
        public async Task<IActionResult> GetSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSnapShotReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSnapShotReport")]
        public async Task<IActionResult> DownloadSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSnapShotReportAsync(query);

                _commonService.SetEmptyString(result.Items.ToList(),
                    nameof(MerchendizingSnapShotReportResultModel.CompetitionDisplay),
                    nameof(MerchendizingSnapShotReportResultModel.GlowSignBoard),
                    nameof(MerchendizingSnapShotReportResultModel.ProductDisplay),
                    nameof(MerchendizingSnapShotReportResultModel.Scheme),
                    nameof(MerchendizingSnapShotReportResultModel.Brochure),
                    nameof(MerchendizingSnapShotReportResultModel.Others));

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Os over 90 days Trend Report
        [HttpGet("OsOver90daysTrendReport")]
        public async Task<IActionResult> OsOver90daysTrendReport([FromQuery] OsOver90daysTrendReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetOsOver90daysTrendReport(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        } 

        [HttpGet("DownloadOsOver90daysTrendReport")]
        public async Task<IActionResult> DownloadOsOver90daysTrendReport([FromQuery] OsOver90daysTrendReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetOsOver90daysTrendReport(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}
