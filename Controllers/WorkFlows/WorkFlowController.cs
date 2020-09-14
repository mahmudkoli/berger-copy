using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.WorkFlows;
using BergerMsfaApi.Services.WorkFlows.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.WorkFlows
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class WorkFlowController : BaseController
    {
        private readonly ILogger<WorkFlowController> _logger;
        private readonly IWorkFlowService _workFlow;

        public WorkFlowController(ILogger<WorkFlowController> logger, IWorkFlowService workflow)
        {
            _logger = logger;
            _workFlow = workflow;
        }

        /// <summary>
        /// Return a list of Workflow Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetWorkFlows()
        {

            try
            {
                var result = await _workFlow.GetWorkFlowsAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("workflowtype")]
        public async Task<IActionResult> GetWorkFlowType()
        {

            try
            {
                var result = await _workFlow.GetWorkFlowTypeAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// Return a list of Workflow Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("workflowtree")]
        public async Task<IActionResult> GetWorkFlowsWithConfiguration()
        {

            try
            {
                var result = await _workFlow.GetWorkFlowsWithConfigAsync();
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkFlow(int id)
        {
            try
            {

                var result = await _workFlow.GetWorkFlowAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }


        /// <summary>
        /// create or update Workflow object and Return a single of Workflow Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        // [ValidationFilter]
        public async Task<IActionResult> SaveWorkFlow([FromBody]WorkFlowModel model)
        {

            try
            {
                var isExist = await _workFlow.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "This workflow code is already exist, try another one!");
                }

                var isExistWorkflowType = await _workFlow.IsWorkflowTypeExistAsync(model.WorkflowType, model.Id);

                if (isExistWorkflowType)
                {
                    ModelState.AddModelError(nameof(model.WorkflowType), "This workflowType is already exist, try another one!");
                }


                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workFlow.SaveAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        
        // /// <summary>
        // /// update Workflow object and Return a single of Workflow Model objects
        // /// </summary>
        // /// <param name="model">WorkFlowModel</param>
        // /// <returns></returns>
        // [HttpPut("update")]
        // [ValidationFilter]
        // public async Task<IActionResult> UpdateWorkflow([FromBody]WorkFlowModel model)
        // {

        //     try
        //     {
        //         var isExist = await _workFlow.IsCodeExistAsync(model.Code, model.Id);
        //         if (isExist)
        //         {
        //             ModelState.AddModelError(nameof(model.Code), " Already Exist");
        //         }
        //         if (!ModelState.IsValid)
        //         {
        //             return ValidationResult(ModelState);
        //         }
        //         else
        //         {
        //             var result = await _workFlow.UpdateAsync(model);
        //             return OkResult(result);
        //         }


        //     }
        //     catch (Exception ex)
        //     {
        //         return ExceptionResult(ex);
        //     }

        // }

        /// <summary>
        /// delete a single workflow object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkFlow(int id)
        {
            try
            {
                var result = await _workFlow.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}