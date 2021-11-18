using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Services.Excel.Interface;
using BergerMsfaApi.Models.Dealer;

namespace BergerMsfaApi.Controllers.Excel
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExcelController : BaseController
    {
        private readonly IExcelReaderService _excelReaderService;
        private readonly IFocusDealerService _focusDealerService;

        public ExcelController(
            IExcelReaderService excelReaderService, 
            IFocusDealerService focusDealerService)
        {
            _excelReaderService = excelReaderService;
            _focusDealerService = focusDealerService;
        }

        [HttpPost("DealerStatusUpdate")]
        public async Task<IActionResult> DealerStatusUpdate([FromForm] DealerStatusExcelImportModel model)
        {
            try
            {
                var responseObj = await _focusDealerService.DealerStatusUpdate(model);
                return OkResult(responseObj);
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
