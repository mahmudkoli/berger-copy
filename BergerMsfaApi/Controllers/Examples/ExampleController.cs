using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Examples;
using BergerMsfaApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Examples
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ExampleController : BaseController
    {
        private readonly ILogger<ExampleController> _logger;
        private readonly IExampleService _example;
        public ExampleController(ILogger<ExampleController> logger
            , IExampleService example)
        {
            _logger = logger;
            _example = example;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
       
        public async Task<IActionResult> GetExamples()
        {

            try
            {
                var result = await _example.GetPagedExamplesAsync(1, 20);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetExample(int id)
        {
            try
            {

                var result = await _example.GetExampleAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
        /// <summary>
        /// create or update Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [ValidationFilter]
        public async Task<IActionResult> SaveExample([FromBody]ExampleModel model)
        {

            try
            {
                var isExist = await _example.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "Code Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _example.SaveAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ValidationFilter]
        public async Task<IActionResult> CreateExample([FromBody]ExampleModel model)
        {
            try
            {
                var isExist = await _example.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "Code Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _example.CreateAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
        /// <summary>
        /// update Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model">ExampleModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ValidationFilter]
        public async Task<IActionResult> UpdateExample([FromBody]ExampleModel model)
        {

            try
            {
                var isExist = await _example.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "Code Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _example.UpdateAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        /// <summary>
        /// delete a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _example.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
