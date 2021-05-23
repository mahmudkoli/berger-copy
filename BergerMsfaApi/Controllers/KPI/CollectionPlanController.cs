using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.KPI
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CollectionPlanController : BaseController
    {
        private readonly ICollectionPlanService _collectionPlanService;

        public CollectionPlanController(ICollectionPlanService collectionPlanService)
        {
            _collectionPlanService = collectionPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _collectionPlanService.GetAllCollectionPlansAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionPlanById(int id)
        {
            try
            {
                var result = await _collectionPlanService.GetCollectionPlansByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollectionPlan([FromBody] SaveCollectionPlanModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (await _collectionPlanService.IsExitsCollectionPlansAsync(model.Id, AppIdentity.AppUser.UserId, model.BusinessArea, model.Territory, DateTime.Now.Year, DateTime.Now.Month))
                    throw new Exception("Already exists collection plan of this area and this month.");
                var result = await _collectionPlanService.AddCollectionPlansAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollectionPlan(int id, [FromBody] SaveCollectionPlanModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                if (await _collectionPlanService.IsExitsCollectionPlansAsync(model.Id, AppIdentity.AppUser.UserId, model.BusinessArea, model.Territory, 0, 0))
                    throw new Exception("Already exists collection plan of this area and this month.");
                var result = await _collectionPlanService.UpdateCollectionPlansAsync(model);
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
                var result = await _collectionPlanService.DeleteCollectionPlansAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("SlippageAmount")]
        public async Task<IActionResult> GetSlippageAmount([FromQuery]CustomerSlippageQueryModel query)
        {
            try
            {
                var result = await _collectionPlanService.GetCustomerSlippageAmountToLastMonth(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
