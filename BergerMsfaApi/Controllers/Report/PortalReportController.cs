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
    }
}
