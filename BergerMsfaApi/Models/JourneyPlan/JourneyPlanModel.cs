using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class JourneyPlanModel
    {
        public int Id { get; set; }
        public string  Code { get; set; }
        public string DealerName { get; set; }
        public int EmployeeRegId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime VisitDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool ApprovedStatus
        {
            get { return (ApprovedBy == null || ApprovedDate == null) ? false : true; }
        }





    }
}