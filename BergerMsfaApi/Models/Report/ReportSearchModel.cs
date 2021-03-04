using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Report
{
    public class ReportBaseSearchModel : QueryObjectModel
    {
        public string DepotId { get; set; }
        //public string DepotName { get; set; }
        public EnumEmployeeRole? EmployeeRole { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }
        public int? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public ReportBaseSearchModel()
        {
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }

    public class LeadSummaryReportSearchModel : ReportBaseSearchModel
    {

    }

    public class LeadGenerationDetailsReportSearchModel : ReportBaseSearchModel
    {
        public string ProjectName { get; set; }
        public int? PaintingStageId { get; set; }
    }

    public class LeadFollowUpDetailsReportSearchModel : ReportBaseSearchModel
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? PaintingStageId { get; set; }
    }
}
