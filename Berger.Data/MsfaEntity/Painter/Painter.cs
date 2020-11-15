using Berger.Data.Common;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Setup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    [Table("Painters")]
    public class Painter :AuditableEntity<int>    {
        public Painter()
        {
           Attachments= new List<Attachment>();
        }
        public string DepotName { get; set; }
        public string SaleGroupCd { get; set; }
        public string TerritroyCd { get; set; }
        public string ZoneCd { get; set; }
        public int PainterCatId { get; set; }
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
        public string Remark { get; set; }
        public decimal AvgMonthlyVal { get; set; }
        public float Loyality { get; set; }
        public int EmployeeId { get; set; }



        //[ForeignKey("SaleGroupCd")]
        //public SaleGroup SaleGroup { get; set; }

        //[ForeignKey("TerritroyCd")]
        //public Territory Territory { get; set; }

        //[ForeignKey("ZoneCd")]
        //public Zone Zone { get; set; }

        //[ForeignKey("PainterCatId")]
        // public DropdownDetail PainterCategory { get; set; }
        //[ForeignKey("AttachedDealerCd")]
        //   public DropdownDetail Dealer { get; set; }

        public List<Attachment> Attachments { get; set; }

    }

  



}
