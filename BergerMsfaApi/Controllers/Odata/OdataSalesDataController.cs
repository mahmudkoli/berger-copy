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
    public class OdataSalesDataController : BaseController
    {
        private readonly ISalesDataService _saledata;
        private readonly IODataService _odataService;

        public OdataSalesDataController(
            ISalesDataService saledata,
            IODataService odataService
            )
        {
            _saledata = saledata;
            _odataService = odataService;
        }

        [HttpGet("InvoiceHistory")]
        public async Task<IActionResult> GetInvoiceHistory([FromQuery] InvoiceHistorySearchModel model)
        {
            try
            {
                var data = await _saledata.GetInvoiceHistory(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("InvoiceItemDetails")]
        public async Task<IActionResult> GetInvoiceItemDetails([FromQuery] InvoiceItemDetailsSearchModel model)
        {
            try
            {
                var data = await _saledata.GetInvoiceItemDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("BrandWiseMTDDetails")]
        public async Task<IActionResult> GetBrandWiseMTDDetails([FromQuery] BrandWiseMTDSearchModel model)
        {
            try
            {
                var data = await _saledata.GetBrandWiseMTDDetails(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("MTSBrandsVolume")]
        public async Task<IActionResult> GetMTSBrandsVolume([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _odataService.GetMTSBrandsVolumeAsync(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
