using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Notification
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IJourneyPlanService _journeyPlanService;
        private readonly IDealerOpeningService _dealerOpeningService;

        public NotificationController(
            ILogger<NotificationController> logger, 
            IJourneyPlanService journeyPlanService, 
            IDealerOpeningService dealerOpeningService)
        {
            _logger = logger;
            _journeyPlanService = journeyPlanService;
            _dealerOpeningService = dealerOpeningService;
        }

        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            try
            {
                NotificationModel notificationVms = new NotificationModel();

                var journeyPlans = await  _journeyPlanService.GetJourneyPlanDetailForLineManagerForNotification();
                foreach (var item in journeyPlans)
                {
                    notificationVms.notificationForJourneyPlan.Add(
                        new NotificationForJourneyPlan()
                        {
                            Name=item.EmployeeName,
                            Code=item.EmployeeId,
                            id=item.Id,
                            Status=item.PlanStatusInText,
                            VisitDate=item.PlanDate
                        });
                }

                var dealeropening = await _dealerOpeningService.GetDealerOpeningPendingListForNotificationAsync();
                if (dealeropening.Count > 0)
                {
                    foreach (var item in dealeropening)
                    {
                        notificationVms.notificationForDealerOpningModel.Add(
                            new NotificationForDealerOpningModel()
                            {
                                Id=item.Id,
                                BusinessArea = item.BusinessArea,
                                CurrentApprovar = item.CurrentApprovar?.UserName,
                                //NextApprovar = item.NextApprovar?.UserName,
                                SaleGroup = item.SaleGroup,
                                SaleOffice = item.SaleOffice,
                                Territory = item.Territory,
                                Zone = item.Zone,
                                Code=item.Code
                            });
                    }
                }
                
                notificationVms.TotalNoification = notificationVms.notificationForDealerOpningModel.Count + notificationVms.notificationForJourneyPlan.Count;

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
