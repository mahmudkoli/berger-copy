using System;
using System.Threading.Tasks;
using Berger.Common;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Examples;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Notification.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Examples
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExampleController : BaseController
    {
        private readonly ILogger<ExampleController> _logger;
        private readonly IExampleService _example;
        private readonly INotificationService _notificationService;
        private readonly ILoginLogService _loginLogService;
        private readonly IEmailSender _emailSender;

        public ExampleController(
            ILogger<ExampleController> logger, 
            IExampleService example,
            INotificationService notificationService,
            ILoginLogService loginLogService, 
            IEmailSender emailSender)
        {
            _logger = logger;
            _example = example;
            _notificationService = notificationService;
            _loginLogService = loginLogService;
            _emailSender = emailSender;
        }

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

        [HttpPost("Email/{email}")]
        public async Task<IActionResult> Email(string email)
        {
            try
            {
                await _emailSender.SendEmailAsync(email, "Berger Test","Berger Test");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("PushNotification")]
        public async Task<IActionResult> PushNotification()
        {
            try
            {
                foreach (var item in (await _loginLogService.GetAllLoggedInUsersAsync()))
                {
                    if (!string.IsNullOrEmpty(item.FCMToken))
                        await _notificationService.SendPushNotificationAsync(item.FCMToken, "Berger Test", "Berger Test");
                }
                return Ok(true);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
