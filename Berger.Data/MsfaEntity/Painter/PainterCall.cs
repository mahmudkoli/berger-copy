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
        public string PainterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int NoOfPainterAttached { get; set; }
        public bool IsAppInstalled { get; set; }
        public float Loyality { get; set; }
        public string AccDbblNumber { get; set; }
        public string AccDbblHolderName { get; set; }
        public string AccChangeReason { get; set; }


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
