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
    public class OdataFinanceDataController : BaseController
    {
        private readonly IFinancialDataService _financialData;

        public OdataFinanceDataController(IFinancialDataService financial)
        {
            _financialData = financial;
        }
        [HttpGet("InvoiceHistory")]
        public async Task<IActionResult> GetInvoiceHistory([FromQuery] InvoiceHistorySearchModel model)
        {
            try
            {
                //var data = await _saledata.GetInvoiceHistory(model);
                var data = true;
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
