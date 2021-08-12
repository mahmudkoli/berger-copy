using BergerMsfaApi.Controllers.Common;
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

        [HttpGet("GetLeadBusinessUpdate")]
        public async Task<IActionResult> GetLeadBusinessUpdate([FromQuery] LeadBusinessReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadBusinessUpdateReportAsync(query);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadBusinessUpdate")]
        public async Task<IActionResult> DownloadLeadBusinessUpdate([FromQuery] LeadBusinessReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadBusinessUpdateReportAsync(query);

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
        public IActionResult GetPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetPainterCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPaintersCall")]
        public IActionResult DownloadPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetPainterCallReportBySp(query);
                return Ok(result);
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
        public IActionResult GetDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetDealerSalesCallReportBySp(query);
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
                var result = await _portalReportService.GetDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerSalesCall")]
        public IActionResult GetSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetSubDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerSalesCall")]
        public IActionResult DownloadSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetSubDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocDealerSalesCall")]
        public IActionResult GetAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetAddhocDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocDealerSalesCall")]
        public IActionResult DownloadAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetAddhocDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocSubDealerSalesCall")]
        public IActionResult GetAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetAddhocSubDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocSubDealerSalesCall")]
        public IActionResult DownloadAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetAddhocSubDealerSalesCallReportBySp(query);

                return Ok(result);
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
        public IActionResult GetSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetSnapShotReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSnapShotReport")]
        public IActionResult DownloadSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetSnapShotReportBySp(query);

                _commonService.SetEmptyString(result.Items.ToList(),
                    nameof(MerchendizingSnapShotReportResultModel.CompetitionDisplay),
                    nameof(MerchendizingSnapShotReportResultModel.GlowSignBoard),
                    nameof(MerchendizingSnapShotReportResultModel.ProductDisplay),
                    nameof(MerchendizingSnapShotReportResultModel.Scheme),
                    nameof(MerchendizingSnapShotReportResultModel.Brochure),
                    nameof(MerchendizingSnapShotReportResultModel.Others));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpGet("GetInactivePainters")]
        public async Task<IActionResult> GetInactivePainters([FromQuery] InactivePainterReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetInactivePainterReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadInactivePainters")]
        public async Task<IActionResult> DownloadInactivePainters([FromQuery] InactivePainterReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetInactivePainterReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
