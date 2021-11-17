using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Services.KPI.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.KPI
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppUniverseReachAnalysisController : BaseController
    {
        private readonly IUniverseReachAnalysisService _universeReachAnalysisService;

        public AppUniverseReachAnalysisController(IUniverseReachAnalysisService universeReachAnalysisService)
        {
            _universeReachAnalysisService = universeReachAnalysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] AppUniverseReachAnalysisQueryObjectModel query)
        {
            try
            {
                var result = await _universeReachAnalysisService.GetAppUniverseReachAnalysissAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUniverseReachAnalysis(int id, [FromBody] SaveAppUniverseReachAnalysisModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _universeReachAnalysisService.UpdateAppUniverseReachAnalysissAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
