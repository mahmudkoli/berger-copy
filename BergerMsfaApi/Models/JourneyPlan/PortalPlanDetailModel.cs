using BergerMsfaApi.Models.Dealer;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class PortalPlanDetailModel
    {
        public PortalPlanDetailModel()
        {
            DealerInfo = new List<DealerInfoModel>();
        }
        public int Id { get; set; }
        public DateTime PlanDate { get; set; }
        public List<DealerInfoModel> DealerInfo { get; set; }
    }

   
}