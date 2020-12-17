using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("getsalesdata")]
        public async Task<IActionResult> Index()
        {
           var x =  _saledata.GetSalesData();
           return View();
        }
    }
}
