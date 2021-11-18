using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Services.KPI.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.KPI
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CollectionConfigController : BaseController
    {
        private readonly ICollectionPlanService _collectionPlanService;

        public CollectionConfigController(
            ICollectionPlanService collectionPlanService)
        {
            _collectionPlanService = collectionPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _collectionPlanService.GetAllCollectionConfigsAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionConfigById(int id)
        {
            try
            {
                var result = await _collectionPlanService.GetCollectionConfigByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollectionConfig(int id, [FromBody] SaveCollectionConfigModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _collectionPlanService.UpdateCollectionConfigAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
