using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.ELearning;
using BergerMsfaApi.Services.ELearning.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.ELearning
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExamController : BaseController
    {
        private readonly IExamService _examService;

        public ExamController(
                IExamService examService
            )
        {
            this._examService = examService;
        }

        [HttpGet("GetAllExamReport")]
        public async Task<IActionResult> GetAllExamReport([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _examService.GetAllExamReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
