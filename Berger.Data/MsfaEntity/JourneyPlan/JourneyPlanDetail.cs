using Berger.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity
{
    public class JourneyPlanDetail: AuditableEntity<int>
    {
        public int DealerId { get; set; }
        //[DataType(DataType.Date)]
        //[Column(TypeName = "Date")]
        //public DateTime VisitDate { get; set; }
        public int PlanId { get; set; }
        [ForeignKey("PlanId")]
        public JourneyPlanMaster JourneyPlanMaster { get; set; }
    }


}
