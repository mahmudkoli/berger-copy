using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.FileImports.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace BergerMsfaApi.Controllers.Users
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;
        private readonly IFileImportService _FileImportService;
        private readonly ICMUserService _User;
        public UserController(ICMUserService userService, ILogger<UserController> logger,
            IFileImportService fileImportService)
        {
            this.logger = logger;
            _FileImportService = fileImportService;
            _User = userService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _User.GetAllUserAsync();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _User.GetUserAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserViewModel model)
        {
            try
            {
                var isExist = await _User.IsUserExistAsync(model.Email, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Email), "User Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _User.CreateUserAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }




        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody]UserViewModel model)
        {
            try
            {
                //var isExist = await _User.IsUserExistAsync(model.Email, model.Id);
                //if (!isExist)
                //{
                //    ModelState.AddModelError(nameof(model.Email), "User Does Not Exist");
                //}
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _User.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _User.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost("excelImport")]
        public async Task<IActionResult> ExcelImportUser([FromForm]IFormFile excelFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _FileImportService.ExcelImportCAUserAsync(excelFile);
                    return OkResult(result.Data, result.Message);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}