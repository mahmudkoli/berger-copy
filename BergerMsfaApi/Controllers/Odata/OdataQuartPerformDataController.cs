using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.OData.Interfaces;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OdataQuartPerformDataController : BaseController
    {
        private readonly IQuarterlyPerformanceDataService _quarterlyPerformanceDataService;

        public OdataQuartPerformDataController(
            IQuarterlyPerformanceDataService quarterlyPerformanceDataService
            )
        {
            _quarterlyPerformanceDataService = quarterlyPerformanceDataService;
        }

        [HttpGet("MTSValueTargetAchivement")]
        public async Task<IActionResult> GetMTSValueTargetAchivement([FromQuery] QuarterlyPerformanceSearchModel model)
        {
            try
            {
                var data = await _quarterlyPerformanceDataService.GetMTSValueTargetAchivement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("EnamelPaintsQuarterlyGrowth")]
        public async Task<IActionResult> GetEnamelPaintsQuarterlyGrowth([FromQuery] OdataQuartPerformSearchModel model)
        {
            try
            {
               var result= await _quarterlyPerformanceDataService.GetEnamelPaintsQuarterlyGrowth(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetPremiumBrandsGrowth")]
        public async Task<IActionResult> GetPremiumBrandsGrowthGrowth([FromQuery] OdataQuartPerformSearchModel model)
        {
            try
            {
               var result= await _quarterlyPerformanceDataService.GetPremiumBrandsGrowth(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        [HttpGet("GetPremiumBrandsContribution")]
        public async Task<IActionResult> GetPremiumBrandsContribution([FromQuery] OdataQuartPerformSearchModel model)
        {
            try
            {
               var result= await _quarterlyPerformanceDataService.GetPremiumBrandsContribution(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
