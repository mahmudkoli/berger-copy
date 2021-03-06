using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Dealer;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class AppJourneyPlanDetailModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public int EditCount { get; set; }
        public string PlanDate { get; set; }
        public Status Status { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public string PlanStatusText { get; set; }
        public List<AppDealerInfoModel> DealerInfoModels { get; set; }

    }
}