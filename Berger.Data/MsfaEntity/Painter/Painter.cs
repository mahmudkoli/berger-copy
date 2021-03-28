using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    [Table("Painters")]
    public class Painter :AuditableEntity<int>    {
        
        public Painter()
        {
            Attachments = new List<PainterAttachment>();
            AttachedDealers = new List<AttachedDealerPainter>();
            PainterCalls = new List<PainterCall>();
    }
        public string Depot { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int PainterCatId { get; set; } // type of client
        public DropdownDetail PainterCat { get; set; } // type of client
        public string PainterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NoOfPainterAttached { get; set; }
        public bool HasDbbl { get; set; }
        public string AccDbblNumber { get; set; }
        public string AccDbblHolderName { get; set; }
        public string PassportNo { get; set; }
        public string NationalIdNo { get; set; }
        public string BrithCertificateNo { get; set; }
        public string PainterImageUrl { get; set; }
        public string AttachedDealerCd { get; set; }
        public bool IsAppInstalled { get; set; }
        public string Remark { get; set; } // app not install reason
        public decimal AvgMonthlyVal { get; set; }
        public float Loyality { get; set; }
        public string EmployeeId { get; set; }

        public List<AttachedDealerPainter> AttachedDealers { get; set; } 
        public List<PainterAttachment> Attachments { get; set; }
        public List<PainterCall> PainterCalls { get; set; }

        public object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }

    public class AttachedDealerPainter: AuditableEntity<int>
    {
        public int Dealer { get; set; }
        public int PainterId { get; set; }

        [ForeignKey("PainterId")]
        public Painter Painter { get; set; }

    }



}
