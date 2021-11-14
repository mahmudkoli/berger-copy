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

        // for portal get to update data
        [HttpGet("GetNewDealerDevelopment")]
        public async Task<IActionResult> GetNewDealerDevelopment([FromQuery] SearchNewDealerDevelopment query)
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

        // for portal report
        [HttpGet("GetDealerOpeningStatus")]
        public async Task<IActionResult> GetDealerOpeningStatus([FromQuery] SearchNewDealerDevelopment query)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetNewDealerDevelopmentReport(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        // for portal report
        [HttpGet("GetDealerConversion")]
        public async Task<IActionResult> GetDealerConversion([FromQuery] SearchNewDealerDevelopment query)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetDealerConversionReport(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        // for portal save data
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
