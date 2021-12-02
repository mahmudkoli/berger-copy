using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.HappyWallet.Lead.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.HappyWallet.Lead
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExternalAppLeadController : BaseController
    {
        private readonly IHappyWalletLeadService _leadService;

        public ExternalAppLeadController(
            IHappyWalletLeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet("GetLeadStatus")]
        public async Task<IActionResult> GetLeadStatusByLeadIds([FromQuery] string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    ModelState.AddModelError("", "Ids are empty.");
                    return ValidationResult(ModelState);
                }

                var idList = ids.Split(',').ToList();
                var result = await _leadService.GetAllHappyWalletLeadsStatusByLeadIdsAsync(idList);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetLeadDetails")]
        public async Task<IActionResult> GetLeadDetailsByLeadIds([FromQuery] string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    ModelState.AddModelError("", "Ids are empty.");
                    return ValidationResult(ModelState);
                }

                var idList = ids.Split(',').ToList();
                var result = await _leadService.GetAllHappyWalletLeadsDetailByLeadIdsAsync(idList);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
