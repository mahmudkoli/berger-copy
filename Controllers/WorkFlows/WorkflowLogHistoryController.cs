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
    public class WorkflowLogHistoryController : BaseController
    {

        private readonly ILogger<WorkflowLogHistoryController> _logger;
        private readonly IWorkFlowLogHistoryService _workflowLogHistory;

        public WorkflowLogHistoryController(ILogger<WorkflowLogHistoryController> logger, IWorkFlowLogHistoryService workflowLogHistory)
        {
            _logger = logger;
            _workflowLogHistory = workflowLogHistory;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetWorkflowLogHistories()
        {

            try
            {
                var result = await _workflowLogHistory.GetWorkFlowLogHistoriesAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }





        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("get-workflow-log-history-for-current-user")]
        public async Task<IActionResult> GetWorkFlowLogHistoryForCurrentUser([FromQuery] int pageNumber, 
            [FromQuery] int pageSize)
        {

            try
            {
                var result = await _workflowLogHistory.GetWorkFlowLogHistoryForCurrentUserAsync(pageNumber, pageSize);
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
        public async Task<IActionResult> GetWorkflowLogHistory(int id)
        {
            try
            {

                var result = await _workflowLogHistory.GetWorkFlowLogHistoryAsync(id);
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
        // [ValidationFilter]
        public async Task<IActionResult> SaveWorkflowLogHistory([FromBody]WorkFlowLogHistoryModel model)
        {

            try
            {
                //var isExist = await _workflowLogHistory.IsWorkFlowLogHistoryExistAsync(model.Id);
                //if (isExist)
                //{
                //    ModelState.AddModelError(nameof(model.Code), "This workflow code is already exist, try another one!");
                //}
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workflowLogHistory.SaveAsync(model);
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
        public async Task<IActionResult> CreateWorkflowLogHistory([FromBody]WorkFlowLogHistoryModel model)
        {
            try
            {
                //var isExist = await _workflowLogHistory.IsCodeExistAsync(model.Code, model.Id);
                //if (isExist)
                //{
                //    ModelState.AddModelError(nameof(model.Code), "Already Exist");
                //}
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workflowLogHistory.CreateAsync(model);
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
        /// <param name="model">WorkFlowLogHistoryModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ValidationFilter]
        public async Task<IActionResult> UpdateWorkflowLogHistory([FromBody]WorkFlowLogHistoryModel model)
        {

            try
            {
                //var isExist = await _workflowLogHistory.IsCodeExistAsync(model.Code, model.Id);
                //if (isExist)
                //{
                //    ModelState.AddModelError(nameof(model.Code), " Already Exist");
                //}
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workflowLogHistory.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteWorkflowLogHistory(int id)
        {
            try
            {
                var result = await _workflowLogHistory.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}