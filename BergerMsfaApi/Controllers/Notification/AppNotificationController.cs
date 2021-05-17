using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using BergerMsfaApi.Services.Notification.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Notification
{
    [Authorize]
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
            this._notificationService = notificationService;
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
