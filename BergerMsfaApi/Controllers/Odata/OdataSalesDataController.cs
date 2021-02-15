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
        private readonly IMTSDataService _mtsdataService;

        public OdataSalesDataController(
            ISalesDataService saledata,
            IMTSDataService mtsdataService
            )
        {
            _saledata = saledata;
            _mtsdataService = mtsdataService;
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
                var data = await _mtsdataService.GetMTSBrandsVolume(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("PremiumBrandPerformance")]
        public async Task<IActionResult> GetPremiumBrandPerformance([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsdataService.GetPremiumBrandPerformance(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("ValueTarget")]
        public async Task<IActionResult> GetValueTarget([FromQuery] MTSSearchModel model)
        {
            try
            {
                var data = await _mtsdataService.GetValueTarget(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
