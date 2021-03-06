using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Sync;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Sync
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SyncController : BaseController
    {
        private readonly IApiSyncService _syncService;

        public SyncController(IApiSyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("getTodaysSalesData")]
        public async Task<IActionResult> GetLeadSummary()
        {
            try
            {
                await _syncService.SyncDailySalesNTargetData();
                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
