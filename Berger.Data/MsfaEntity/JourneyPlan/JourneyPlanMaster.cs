using Berger.Common.Enumerations;
using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity
{
    public class JourneyPlanMaster: AuditableEntity<int>
    {
        public JourneyPlanMaster()
        {
            JourneyPlanDetail = new List<JourneyPlanDetail>();
            PlanStatus = PlanStatus.Pending;
        }
        public string EmployeeId { get; set; }
        public string LineManagerId { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime  PlanDate { get; set; }
        public string Comment { get; set; }
        public int ApprovedById { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool IsActive { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public int RejectedBy { get; set; }
        public  DateTime RejectedDate { get; set; }
        public List<JourneyPlanDetail> JourneyPlanDetail { get; set; }
    }


}
