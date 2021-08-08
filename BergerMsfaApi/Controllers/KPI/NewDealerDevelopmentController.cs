using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.KPI;
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
    public class NewDealerDevelopmentController : BaseController
    {
        private readonly INewDealerDevelopmentService _newDealerDevelopmentService;

        public NewDealerDevelopmentController(
            INewDealerDevelopmentService newDealerDevelopmentService)
        {
            _newDealerDevelopmentService = newDealerDevelopmentService;
        }


        [HttpGet("GetNewDealerDevelopment")]
        public async Task<IActionResult> GetCollectionConfigById([FromQuery] SearchNewDealerDevelopment query)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetNewDealerDevelopmentByIdAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost("SaveNewDealerDevelopment")]
        public async Task<IActionResult> SaveNewDealerDevelopment(IList<NewDealerDevelopment> data)
        {
            try
            {
                var result = await _newDealerDevelopmentService.AddNewDealerDevelopmentAsync(data);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCollectionConfig(int id, [FromBody] SaveCollectionConfigModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationResult(ModelState);
        //        var result = await _collectionPlanService.UpdateCollectionConfigAsync(model);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}
    }
}
