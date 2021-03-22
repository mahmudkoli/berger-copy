using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Report
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class PortalReportController : BaseController
    {
        private readonly IPortalReportService _portalReportService;

        public PortalReportController(
                IPortalReportService portalReportService
            )
        {
            this._portalReportService = portalReportService;
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

        #endregion

    }
}
