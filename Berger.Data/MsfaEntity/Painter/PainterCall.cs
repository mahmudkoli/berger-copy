using Berger.Data.Common;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Setup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class PainterCall:AuditableEntity<int>
    {
        public PainterCall()
        {
            PainterCompanyMTDValue = new List<PainterCompanyMTDValue>();
            AttachedDealers = new List<AttachedDealerPainterCall>();
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
        public bool HasDbbl { get; set; }
        public int PainterCatId { get; set; }
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
        
        public List<AttachedDealerPainterCall> AttachedDealers { get; set; }
        public List<PainterCompanyMTDValue> PainterCompanyMTDValue { get; set; }
    }

    public class AttachedDealerPainterCall : AuditableEntity<int>
    {
        public int DealerId { get; set; } // DealerInfo Id
        
        [ForeignKey("DealerId")]
        public DealerInfo Dealer { get; set; }
        
        public int PainterCallId { get; set; }

        [ForeignKey("PainterCallId")]
        public PainterCall PainterCall { get; set; }
    }
}
