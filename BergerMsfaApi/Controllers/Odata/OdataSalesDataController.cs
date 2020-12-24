using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;
using Berger.Odata.Services;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OdataSalesDataController : Controller
    {
        private readonly ISalesData _saledata;

        public OdataSalesDataController(ISalesData saledata)
        {
            _saledata = saledata;
        }

        [HttpPost("invoicesummary")]
        public async Task<IActionResult> GetInvoiceHistory(SalesDataSearchModel model)
        {
           var x =  _saledata.GetInvoiceHistory(model);
           return Ok(x);
        }

        [HttpPost("invoicedetails")]
        public async Task<IActionResult> GetInvoiceDetails(SalesDataSearchModel model)
        {
            var x = _saledata.GetInvoiceDetails(model);
            return Ok(x);
        }

        [HttpPost("invoiceitemdetails")]
        public async Task<IActionResult> GetInvoiceItemDetails(SalesDataSearchModel model)
        {
            var x = _saledata.GetInvoiceItemDetails(model);
            return Ok(x);
        }

        [HttpPost("brandwisemtddetails")]
        public async Task<IActionResult> GetBrandwiseMTDetails(SalesDataSearchModel model)
        {
            
            var x = _saledata.GetBrandWiseMTDDetails(model);
            return Ok(x);
        }
    }
}
