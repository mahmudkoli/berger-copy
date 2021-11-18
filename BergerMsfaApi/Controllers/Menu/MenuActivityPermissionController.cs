using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Menus;
using BergerMsfaApi.Services.Menus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Menu
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class MenuActivityPermissionController : BaseController
    {
        private readonly ILogger<MenuActivityPermissionController> _logger;
        private readonly IMenuActivityPermissionService _menuActivityPermissionService;

        public MenuActivityPermissionController(
            ILogger<MenuActivityPermissionController> logger,
            IMenuActivityPermissionService menuActivityPermissionservice)
        {
            _menuActivityPermissionService = menuActivityPermissionservice;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllMenuActivityPermissionAsync()
        {
            try
            {
                var result = await _menuActivityPermissionService.GetAllMenusActivityPermissionAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("activity_permission_by_role_id/{id}")]
        public async Task<IActionResult> GetAllMenuActivityPermissionByRoleIdAsync(int id)
        {
            try
            {
                var result = await _menuActivityPermissionService.GetAllMenusActivityPermissionByRoleIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMenu([FromBody]MenuActivityPermissionModel model)
        {
            try
            {
                var isExist = await _menuActivityPermissionService.IsMenuActivityPermissionExistAsync(model.RoleId, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Role.Name), "Permission Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menuActivityPermissionService.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMenu([FromBody]MenuActivityPermissionModel model)
        {
            try
            {
                var isExist = await _menuActivityPermissionService.IsMenuActivityPermissionExistAsync(model.RoleId, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Role.Name), "Activity Already Exist");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menuActivityPermissionService.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("createorupdateall")]
        public async Task<IActionResult> CreateOrUpdateAllMenuActivityPermission([FromBody] List<MenuActivityPermissionVm> modelList)
        {
            try
            {
                var result = await _menuActivityPermissionService.CreateOrUpdateAllAsync(modelList);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMenuActivityPermission(int id)
        {
            try
            {
                var result = await _menuActivityPermissionService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("get-activity-permission/{roleId}")]
        public async Task<IActionResult> GetActivityPermissions(int roleId)
        {
            try
            {
                var result = await _menuActivityPermissionService.GetActivityPermissions(roleId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
