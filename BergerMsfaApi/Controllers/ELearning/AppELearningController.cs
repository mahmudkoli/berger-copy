using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.ELearning.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.ELearning
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppELearningController : BaseController
    {
        private readonly IELearningService _eLearningService;

        public AppELearningController(
            IELearningService eLearningService)
        {
            _eLearningService = eLearningService;
        }

        //TODO: need to change the endpoint
        [HttpGet("GetAllELearning")]
        public async Task<IActionResult> GetAllELearning()
        {
            try
            {
                var result = await _eLearningService.GetAllActiveAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetAllELearningByCategoryId/{id}")]
        public async Task<IActionResult> GetAllELearningByCategoryId(int id)
        {
            try
            {
                var result = await _eLearningService.GetAllActiveByCategoryIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetELearningById/{id}")]
        public async Task<IActionResult> GetELearningById(int id)
        {
            try
            {
                var result = await _eLearningService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
