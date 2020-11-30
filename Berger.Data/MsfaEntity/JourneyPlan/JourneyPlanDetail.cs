using Berger.Data.Common;
using System;

namespace Berger.Data.MsfaEntity
{
    public class JourneyPlanDetail: AuditableEntity<int>
    {
        public int DealerId { get; set; }
        public DateTime VisitDate { get; set; }
        public int PlanId { get; set; }
        public JourneyPlanMaster JourneyPlanMaster { get; set; }
    }


}
