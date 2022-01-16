using System;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Services.KPI.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.KPI
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class UniverseReachAnalysisController : BaseController
    {
        private readonly IUniverseReachAnalysisService _universeReachAnalysisService;

        public UniverseReachAnalysisController(IUniverseReachAnalysisService universeReachAnalysisService)
        {
            _universeReachAnalysisService = universeReachAnalysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] UniverseReachAnalysisQueryObjectModel query)
        {
            try
            {
                var result = await _universeReachAnalysisService.GetAllUniverseReachAnalysissByCurrentUserAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniverseReachAnalysisById(int id)
        {
            try
            {
                var result = await _universeReachAnalysisService.GetUniverseReachAnalysissByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniverseReachAnalysis([FromBody] SaveUniverseReachAnalysisModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var fiscalYear = _universeReachAnalysisService.GetCurrentFiscalYear();
                if (await _universeReachAnalysisService.IsExitsUniverseReachAnalysissAsync(model.Id, model.BusinessArea, model.Territory, fiscalYear))
                    throw new Exception("Already exists Universe/Reach Plan of this area and this fiscal year.");
                var result = await _universeReachAnalysisService.AddUniverseReachAnalysissAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUniverseReachAnalysis(int id, [FromBody] SaveUniverseReachAnalysisModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (await _universeReachAnalysisService.IsExitsUniverseReachAnalysissAsync(model.Id, model.BusinessArea, model.Territory))
                    throw new Exception("Already exists Universe/Reach Plan of this area and this fiscal year.");
                var result = await _universeReachAnalysisService.UpdateUniverseReachAnalysissAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchemeDetail(int id)
        {
            try
            {
                var result = await _universeReachAnalysisService.DeleteUniverseReachAnalysissAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
