using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OdataFinancialDataController : BaseController
    {
        private readonly IFinancialDataService _financialData;

        public OdataFinancialDataController(IFinancialDataService financial)
        {
            _financialData = financial;
        }

        [HttpGet("CollectionHistory")]
        public async Task<IActionResult> GetCollectionHistory([FromQuery] CollectionHistorySearchModel model)
        {
            try
            {
                var data = await _financialData.GetCollectionHistory(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingDetails")]
        public async Task<IActionResult> GetOutstandingDetails([FromQuery] OutstandingDetailsSearchModel model)
        {
            try
            {
                var data = await _financialData.GetOutstandingDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("OutstandingSummary")]
        public async Task<IActionResult> GetOutstandingSummary([FromQuery] OutstandingSummarySearchModel model)
        {
            try
            {
                var data = await _financialData.GetOutstandingSummary(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
