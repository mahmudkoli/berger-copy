using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Services.Excel.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Controllers.Excel
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExcelController : BaseController
    {
        private readonly IExcelReaderService _excelReaderService;

        public ExcelController(IExcelReaderService excelReaderService)
        {
            _excelReaderService = excelReaderService;
        }

        [AllowAnonymous]
        [HttpPost("SubmitExcel")]
        public async Task<IActionResult> SubmitExcel(IFormFile file)
        {
            try
            {
               await _excelReaderService.LoadDataAsync(file);
               return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }

        }
    }
}
