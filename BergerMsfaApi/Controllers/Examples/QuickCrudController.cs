using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Examples;
using GenericServices;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Examples
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class QuickCrudController : BaseController
    {
        private readonly ICrudServicesAsync _service;

        public QuickCrudController(
            ICrudServicesAsync service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var result = await _service.ReadSingleAsync()
                return OkResult("result");
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        [ValidationFilter]
        public async Task<IActionResult> Create([FromBody] QuickCrudModel model)
        {
            await _service.CreateAndSaveAsync(model);
            return OkResult("");
        }
    }
}
