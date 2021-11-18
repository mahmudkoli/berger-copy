using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.AlertNotification;
using BergerMsfaApi.Services.Notification.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Notification
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppNotificationController : BaseController
    {
        private readonly ILogger<AppNotificationController> _logger;
        private readonly INotificationService _notificationService;
        private readonly IAlertNotificationService _alertNotificationService;

        public AppNotificationController(
            ILogger<AppNotificationController> logger,
            INotificationService notificationService,
            IAlertNotificationService alertNotificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _alertNotificationService = alertNotificationService;
            
        }

        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            try
            {
                var result = await _notificationService.GetAllTodayNotification(AppIdentity.AppUser.UserId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetAlertNotificationByEmpRole")]
        public async Task<IActionResult> GetAlertNotificationByEmpRole()
        {
            try
            {
                
                var result = await _alertNotificationService.GetNotificationByEmpRole();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
