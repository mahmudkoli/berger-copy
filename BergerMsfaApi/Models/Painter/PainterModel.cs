using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Models.Painter;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterModel
    {
        public PainterModel()
        {
            AttachedDealers = new List<int>();
            Attachments = new List<PainterAttachmentModel>();
        }
        public int Id { get; set; }
        public string DepotName { get; set; }
        public string Depot { get; set; }
        public string SaleGroupName { get; set; }
        public string SaleGroup { get; set; }
        public string TerritoryName { get; set; }
        public string Territory { get; set; }
        public string ZoneName { get; set; }
        public string Zone { get; set; }

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
        [RegularExpression(
            "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$",ErrorMessage ="painter image must be properly base64 formatted.")]
        public string PainterImageUrl { get; set; }
        public int PainterCatId { get; set; }
        public string PainterCatName { get; set; }
        public bool IsAppInstalled { get; set; }
        public string Remark { get; set; }
        public decimal AvgMonthlyVal { get; set; }
        public float Loyality { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public List<PainterAttachmentModel> Attachments { get; set; }
        public List<int> AttachedDealers { get; set; }


    }

    
}
