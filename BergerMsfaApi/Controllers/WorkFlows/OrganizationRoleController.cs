using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Organizations;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Organizations.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.WorkFlows
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class OrganizationRoleController : BaseController
    {
        private readonly ILogger<OrganizationRoleController> _logger;
        private readonly IOrganizationRoleService _OrganizationRole;
       
        public OrganizationRoleController(ILogger<OrganizationRoleController> logger, IOrganizationRoleService OrganizationRole)
        {
            _logger = logger;
            _OrganizationRole = OrganizationRole;
        }

        /// <summary>
        /// Return a list of OrganizationRole Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetOrganizationRoles()
        {
            try
            {
                var result = await _OrganizationRole.GetPagedOrganizationRolesAsync(1, 20);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //treview
        [HttpGet("orgusertree")]
        public async Task<IActionResult> GetOrganizationUser()
        {

            try
            {
                var result = await _OrganizationRole.GetOrganizationUserRoleAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("getuserbyrol/{orgroleId}")]
        public async Task<IActionResult> GetOrganizationUser(int orgroleId)
        {

            try
            {
                var result = await _OrganizationRole.GetOrgenizationUserByRole(orgroleId);
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
        /// <returns>ApiResponse</returns>na
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizationRole(int id)
        {
            try
            {
                var result = await _OrganizationRole.GetOrganizationRoleAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create or update OrganizationRole object and Return a single of OrganizationRole Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveOrganizationRole([FromBody]OrganizationRoleModel model)
        {
            try
            {
                var isExist = await _OrganizationRole.IsOrganizationRoleExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "This Organization Role is already exist, please try another Organization Role Name.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _OrganizationRole.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        
        /// <summary>
        /// update OrganizationRole object and Return a single of OrganizationRole Model objects
        /// </summary>
        /// <param name="model">OrganizationRoleModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrganizationRole([FromBody]OrganizationRoleModel model)
        {
            try
            {
                var isExist = await _OrganizationRole.IsOrganizationRoleExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "This Organization Role is already exist, please try another Organization Role Name.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _OrganizationRole.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteOrganizationRole(int id)
        {
            try
            {
                var result = await _OrganizationRole.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// create User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("rolelinkwithuser")]
        public async Task<IActionResult> RoleLinkWithUser([FromBody]UserRoleMappingModel model)
        {
            try
            {
                var isExist = await _OrganizationRole.IsOrganizationRoleLinkWithUserExistAsync(model.RoleId, model.UserInfoId);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.RoleId), "This User has already in this organization role");
                }
                var result = await _OrganizationRole.SaveOrganizationRoleLinkWithUserAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
    }
}