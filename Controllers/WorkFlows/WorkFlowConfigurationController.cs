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
    public class WorkFlowConfigurationController : BaseController
    {
        private readonly ILogger<Controllers.WorkFlows.WorkFlowConfigurationController> _logger;
        private readonly IWorkFlowConfigurationService _WorkFlowConfiguration;

        public WorkFlowConfigurationController(ILogger<Controllers.WorkFlows.WorkFlowConfigurationController> logger, IWorkFlowConfigurationService WorkFlowConfiguration)
        {
            _logger = logger;
            _WorkFlowConfiguration = WorkFlowConfiguration;
        }

        /// <summary>
        /// Return a list of WorkFlowConfiguration Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetWorkFlowConfigurations()
        {
            try
            {
                var result = await _WorkFlowConfiguration.GetPagedQueryWorkFlowConfigurationsAsync(1, 20);
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
        public async Task<IActionResult> GetWorkFlowConfiguration(int id)
        {
            try
            {
                var result = await _WorkFlowConfiguration.GetWorkFlowConfigurationAsync(id);
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
        [HttpGet("configlistbyworkflow/{id}")]
        public async Task<IActionResult> GetWorkFlowConfigurationByWorkfloId(int id)
        {
            try
            {
                var result = await _WorkFlowConfiguration.GetWorkFlowConfigurationsByWorkflowIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create or update WorkFlowConfiguration object and Return a single of WorkFlowConfiguration Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveWorkFlowConfiguration([FromBody]WorkFlowConfigurationModel model)
        {
            try
            {
        
                var isExistOrgRoleId = await _WorkFlowConfiguration.IsOrganizationRoleExistAsync(model.MasterWorkFlowId, model.OrgRoleId, model.Id);
                if (isExistOrgRoleId)
                {
                    ModelState.AddModelError(nameof(model.OrgRoleId), "Organization Role Already Exist");
                }
                var isExistSequence = await _WorkFlowConfiguration.IsSequenceExistAsync(model.MasterWorkFlowId, model.sequence);
                if (isExistSequence)
                {
                    ModelState.AddModelError(nameof(model.MasterWorkFlowId), "Sequence Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _WorkFlowConfiguration.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        /// <summary>
        /// update WorkFlowConfiguration object and Return a single of WorkFlowConfiguration Model objects
        /// </summary>
        /// <param name="model">WorkFlowConfigurationModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateWorkFlowConfiguration([FromBody]WorkFlowConfigurationModel model)
        {
            try
            {
 

                var isExistOrgRoleId = await _WorkFlowConfiguration.IsOrganizationRoleExistAsync(model.MasterWorkFlowId, model.OrgRoleId, model.Id);
                if (isExistOrgRoleId)
                {
                    ModelState.AddModelError(nameof(model.OrgRoleId), "Organization Role already exist with this Workflow!");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }

                else
                {
                    var result = await _WorkFlowConfiguration.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteWorkFlowConfiguration(int id)
        {
            try
            {
                var result = await _WorkFlowConfiguration.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}