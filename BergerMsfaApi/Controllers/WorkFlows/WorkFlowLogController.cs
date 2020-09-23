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
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class WorkFlowLogController : BaseController
    {
        private readonly ILogger<WorkFlowLogController> _logger;
        private readonly IWorkFlowLogService _workFlowLog;

        public WorkFlowLogController(ILogger<WorkFlowLogController> logger,
            IWorkFlowLogService WorkFlowLog)
        {
            _logger = logger;
            _workFlowLog = WorkFlowLog;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetWorkFlowLogs()
        {

            try
            {
                var result = await _workFlowLog.GetWorkFlowLogsAsync();
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
        [HttpGet("get-workflow-log-for-current-user")]
        public async Task<IActionResult> GetWorkFlowLogForCurrentUser([FromQuery] int? status,
            [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {

            try
            {
                var result = await _workFlowLog.GetWorkFlowLogForCurrentUserAsync(status, pageNumber, pageSize);
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
        public async Task<IActionResult> GetWorkFlowLog(int id)
        {
            try
            {
                var result = await _workFlowLog.GetWorkFlowLogAsync(id);
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
        public async Task<IActionResult> SaveWorkFlowLog([FromBody] WorkFlowLogModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workFlowLog.SaveAsync(model);
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
        public async Task<IActionResult> CreateWorkFlowLog([FromBody] WorkFlowLogModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workFlowLog.CreateAsync(model);
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
        /// <param name="model">WorkFlowLogModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ValidationFilter]
        public async Task<IActionResult> UpdateWorkFlowLog([FromBody] WorkFlowLogModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _workFlowLog.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteWorkFlowLog(int id)
        {
            try
            {
                var result = await _workFlowLog.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}