using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Menus;
using BergerMsfaApi.Services.Menus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Menu
{
    
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]  
    [Route("api/v{v:apiVersion}/[controller]")]
    public class MenuActivityController : BaseController 
    {
        private readonly ILogger<MenuActivityController> _logger;
        private readonly IMenuActivityService _menuActivityService;
        public MenuActivityController(ILogger<MenuActivityController> logger, IMenuActivityService menuActivityService)
        {
            _logger = logger;
            _menuActivityService = menuActivityService;
        }
    

        [HttpGet("")]
        public async Task<IActionResult> GetAllMenuActivity()
        {
            try
            {
                var result = await _menuActivityService.GetAllMenusActivityAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuActivityById(int id)
        {
            try
            {
                var result = await _menuActivityService.GetMenuActivityAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("get-all/{id}")]
        public async Task<IActionResult> GetAllMenuActivityById(int id)
        {
            try
            {
                var result = await _menuActivityService.GetAllMenuActivityById(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateMenu([FromBody]MenuActivityModel model)
        {
            try
            {
                var isExist = await _menuActivityService.IsMenuActivityExistAsync(model.Name, model.ActivityCode, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "Activity Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menuActivityService.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMenu([FromBody]MenuActivityModel model)
        {
            try
            {
                var isExist = await _menuActivityService.IsMenuActivityExistAsync(model.Name, model.ActivityCode, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "Activity Already Exist");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menuActivityService.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var result = await _menuActivityService.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// This gets all the menu from menu permission table
        /// table where roleid matches
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///
        [HttpGet("get-all-menu-activity-permissions-by-roleid/{id}")]
        public async Task<IActionResult> GetAllMenuActivityPermissionsByRoleId(int id) 
        {
            try
            {
                var result = await _menuActivityService.GetAllMenuActivityPermissionByRoleId(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

    }
}
