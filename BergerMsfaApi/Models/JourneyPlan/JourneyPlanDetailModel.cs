using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Dealer;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class JourneyPlanDetailModel 
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public EmployeeModel  Employee{ get; set; }
        public DateTime PlanDate { get; set; }
        public Status Status { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public List<DealerInfoModel> DealerInfoModels { get; set; }
  
    }
    public class EmployeeModel
    {
        public string FirstName { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
    }
}