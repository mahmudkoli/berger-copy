using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.Notification.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Notification
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppNotificationController : BaseController
    {
        private readonly ILogger<AppNotificationController> _logger;
        private readonly INotificationService _notificationService;

        public AppNotificationController(
            ILogger<AppNotificationController> logger,
            INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
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
    }
}
