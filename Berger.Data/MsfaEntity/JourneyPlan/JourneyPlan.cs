using Berger.Common.Enumerations;
using Berger.Data.Common;
using System;
using System.Collections.Generic;

namespace Berger.Data.MsfaEntity
{
    public class JourneyPlan: AuditableEntity<int>
    {
        public string Code { get; set; }
        public int EmployeeRegId { get; set; }
        public DateTime VisitDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class JourneyPlanDetail: AuditableEntity<int>
    {
        public int DealerId { get; set; }
        public DateTime VisitDate { get; set; }
        public int PlanId { get; set; }
        public JourneyPlanMaster JourneyPlanMaster { get; set; }
    }

    public class JourneyPlanMaster: AuditableEntity<int>
    {
        public JourneyPlanMaster()
        {
            JourneyPlanDetail = new List<JourneyPlanDetail>();
            PlanStatus = PlanStatus.Pending;
        }
        public string EmployeeId { get; set; }
        public string LineManagerId { get; set; }
        public DateTime  PlanDate { get; set; }
        public int ApprovedById { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool IsActive { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public int RejectedBy { get; set; }
        public  DateTime RejectedDate { get; set; }
        public List<JourneyPlanDetail> JourneyPlanDetail { get; set; }
    }


}
