using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Dealer;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class AppJourneyPlanDetailModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PlanDate { get; set; }
        public Status Status { get; set; }
        public List<AppDealerInfoModel> DealerInfoModels { get; set; }

    }
}