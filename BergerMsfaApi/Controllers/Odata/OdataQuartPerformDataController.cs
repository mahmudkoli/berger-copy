using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;

namespace BergerMsfaApi.Controllers.Odata
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OdataQuartPerformDataController : BaseController
    {
        private readonly IQuarterlyPerformanceDataService _quarterlyPerformanceDataService;

        public OdataQuartPerformDataController(
            IQuarterlyPerformanceDataService quarterlyPerformanceDataService)
        {
            _quarterlyPerformanceDataService = quarterlyPerformanceDataService;
        }

        [HttpGet("QuarterlyPerformanceReport")]
        public async Task<IActionResult> GetMTSValueTargetAchivement([FromQuery] QuarterlyPerformanceSearchModel model)
        {
            try
            {
                var result = new List<AppQuarterlyPerformanceDataResultModel>();

                if (model.QuarterlyPerformanceType == EnumQuarterlyPerformanceModel.MTSValueTargetAchivement)
                {
                    var data = await _quarterlyPerformanceDataService.GetMTSValueTargetAchivement(model);
                    result = data.ToList();
                } 
                else if (model.QuarterlyPerformanceType == EnumQuarterlyPerformanceModel.BillingDealerQuarterlyGrowth)
                {
                    var data = await _quarterlyPerformanceDataService.GetBillingDealerQuarterlyGrowth(model);
                    result = data.ToList();
                }
                else if (model.QuarterlyPerformanceType == EnumQuarterlyPerformanceModel.EnamelPaintsQuarterlyGrowt)
                {
                    var data = await _quarterlyPerformanceDataService.GetEnamelPaintsQuarterlyGrowth(model);
                    result = data.ToList();
                }
                else if (model.QuarterlyPerformanceType == EnumQuarterlyPerformanceModel.PremiumBrandsGrowth)
                {
                    var data = await _quarterlyPerformanceDataService.GetPremiumBrandsGrowth(model);
                    result = data.ToList();
                }
                else if (model.QuarterlyPerformanceType == EnumQuarterlyPerformanceModel.PremiumBrandsContribution)
                {
                    var data = await _quarterlyPerformanceDataService.GetPremiumBrandsContribution(model);
                    result = data.ToList();
                }

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
