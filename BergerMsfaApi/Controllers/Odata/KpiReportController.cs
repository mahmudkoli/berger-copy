using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class KpiReportController : BaseController
    {
        private readonly IKpiDataService _kpiDataService;

        public KpiReportController(IKpiDataService kpiDataService)
        {
            _kpiDataService = kpiDataService;
        }

        [HttpGet("GetTerritoryTargetAchivement")]
        public async Task<IActionResult> GetTerritoryTargetAchivement([FromQuery] TerritoryTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetTerritoryTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadTerritoryTargetAchivement")]
        public async Task<IActionResult> DownloadTerritoryTargetAchivement([FromQuery] TerritoryTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetTerritoryTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerWiseTargetAchivement")]
        public async Task<IActionResult> GetDealerWiseTargetAchivement([FromQuery] DealerWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetDealerWiseTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerWiseTargetAchivement")]
        public async Task<IActionResult> DownloadDealerWiseTargetAchivement([FromQuery] DealerWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetDealerWiseTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetProductWiseTargetAchivement")]
        public async Task<IActionResult> GetProductWiseTargetAchivement([FromQuery] ProductWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetProductWiseTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadProductWiseTargetAchivement")]
        public async Task<IActionResult> DownloadProductWiseTargetAchivement([FromQuery] ProductWiseTargetAchievementSearchModel model)
        {
            try
            {
                var data = await _kpiDataService.GetProductWiseTargetAchivement(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
