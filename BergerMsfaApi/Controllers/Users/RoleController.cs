using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Users
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class RoleController : BaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _Role;

        public RoleController(
            ILogger<RoleController> logger, 
            IRoleService Role)
        {
            _logger = logger;
            _Role = Role;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await _Role.GetPagedRolesAsync(1,20);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var result = await _Role.GetRoleAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveRole([FromBody]RoleModel model)
        {
            try
            {
                var isExist = await _Role.IsRoleExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "This Role Name is already exist, please try another Role Name.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Role.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody]RoleModel model)
        {
            try
            {
                var isExist = await _Role.IsRoleExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "This Role Name is already exist, please try another Role Name.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Role.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromBody]RoleModel model)
        {
            try
            {
                var isExist = await _Role.IsRoleExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "This Role Name is already exist, please try another Role Name.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Role.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _Role.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}