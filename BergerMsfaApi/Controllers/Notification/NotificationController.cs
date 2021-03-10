using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Notification
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IJourneyPlanService _journeyPlanService;
        //private readonly IMenuService _menu;
        //private readonly IMenuService _menu;
        public NotificationController(ILogger<NotificationController> logger, IJourneyPlanService journeyPlanService)
        {
            _logger = logger;
            _journeyPlanService = journeyPlanService;
        }

        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            try
            {
                List<NotificationModel> notificationVms = new List<NotificationModel>();

                var result = await  _journeyPlanService.GetJourneyPlanDetailForLineManagerForNotification();
                foreach (var item in result)
                {
                    notificationVms.Add(
                        new NotificationModel()
                        {
                            Name=item.EmployeeName,
                            Code=item.EmployeeId,
                            id=item.Id,
                            Status=item.PlanStatusInText,
                            VisitDate=item.PlanDate
                        }
                        );
                }
                //return OkResult(result);
                return Ok(notificationVms);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
