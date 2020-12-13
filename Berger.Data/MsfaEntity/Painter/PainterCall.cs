using Berger.Data.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class PainterCall:AuditableEntity<int>
    {
        public PainterCall()
        {
            PainterCompanyMTDValue = new List<PainterCompanyMTDValue>();
        }
        public bool HasSchemeComnunaction { get; set; }
        public bool HasPremiumProtBriefing { get; set; }
        public bool HasNewProBriefing { get; set; }
        public bool HasUsageEftTools { get; set; }
        public bool HasAppUsage { get; set; }
        public decimal WorkInHandNumber { get; set; }
        public bool HasDbblIssue { get; set; }
        public string Comment { get; set; }
        public int PainterId { get; set; }
        [ForeignKey("PainterId")]
        public Painter Painter { get; set; }
        public string EmployeeId { get; set; }
        public List<PainterCompanyMTDValue> PainterCompanyMTDValue { get; set; }
    }
}
