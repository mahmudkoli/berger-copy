using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Sync;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.Setup.Interfaces;

namespace BergerMsfaApi.Controllers.Sync
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SyncSetupController : BaseController
    {
        private readonly ISyncSetupService _syncSetupService;


        public SyncSetupController(ISyncSetupService syncSetupService)
        {
            _syncSetupService = syncSetupService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _syncSetupService.GetAll();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _syncSetupService.GetById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(SyncSetup syncSetup)
        {
            try
            {
                await _syncSetupService.Update(syncSetup);
                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
    }
}
