using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Notification
{
    public class NotificationModel
    {
        public NotificationModel()
        {
            notificationForJourneyPlan = new List<NotificationForJourneyPlan>();
            notificationForDealerOpningModel = new List<NotificationForDealerOpningModel>();
        }
        public List<NotificationForJourneyPlan> notificationForJourneyPlan { get; set; }
        public List<NotificationForDealerOpningModel> notificationForDealerOpningModel { get; set; }

        public int TotalNoification { get; set; }
    }

    public class NotificationForDealerOpningModel
    {
        public int Id { get; set; }

        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CurrentApprovar { get; set; }
        public string NextApprovar { get; set; }
    }

    public class NotificationForJourneyPlan
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string VisitDate { get; set; }
    }
}
