using Berger.Data.Common;
using System;

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


}
