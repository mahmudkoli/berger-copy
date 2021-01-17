using BergerMsfaApi.Controllers.Common;
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
    public class AppExamController : BaseController
    {
        private readonly IExamService _examService;

        public AppExamController(
                IExamService examService
            )
        {
            this._examService = examService;
        }

        [HttpGet("GetAllQuestionSet")]
        public async Task<IActionResult> GetAllQuestionSet()
        {
            try
            {
                var result = await _examService.GetAllQuestionSetAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetAllQuestionByQuestionSetId/{id}")]
        public async Task<IActionResult> GetAllQuestionByQuestionSetId(int id)
        {
            try
            {
                var result = await _examService.GetAllQuestionByQuestionSetIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("SaveQuestionAnswer")]
        public async Task<IActionResult> SaveQuestionAnswer([FromBody] AppQuestionSetModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _examService.SaveQuestionAnswerAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
