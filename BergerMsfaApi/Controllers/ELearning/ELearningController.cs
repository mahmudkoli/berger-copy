using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.ELearning;
using BergerMsfaApi.Services.ELearning.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.ELearning
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ELearningController : BaseController
    {
        private readonly IELearningService _eLearningService;

        public ELearningController(
            IELearningService eLearningService)
        {
            this._eLearningService = eLearningService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _eLearningService.GetAllAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _eLearningService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SaveELearningDocumentModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _eLearningService.AddAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SaveELearningDocumentModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _eLearningService.UpdateAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _eLearningService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("select")]
        public async Task<IActionResult> GetAllForSelect()
        {
            try
            {
                var result = await _eLearningService.GetAllForSelectAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("AddAttachmentFile")]
        public async Task<IActionResult> AddAttachmentFile([FromForm] SaveELearningAttachmentModel model)
        {
            try
            {
                var result = await _eLearningService.AddAttachmentAsync(model.ELearningDocumentId, model.AttachmentFile);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("AddAttachmentLink")]
        public async Task<IActionResult> AddAttachmentLink([FromBody] SaveELearningAttachmentModel model)
        {
            try
            {
                var result = await _eLearningService.AddAttachmentAsync(model.ELearningDocumentId, model.AttachmentLink);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteAttachment/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            try
            {
                var result = await _eLearningService.DeleteAttachmentAsync(attachmentId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
