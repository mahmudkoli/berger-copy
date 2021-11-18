using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Sync;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using BergerMsfaApi.Filters;

namespace BergerMsfaApi.Controllers.Sync
{
    [AuthorizeFilter]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
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
