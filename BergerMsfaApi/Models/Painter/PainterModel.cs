using System.Collections.Generic;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterModel
    {
        public PainterModel()
        {
            Attachments = new List<string>();
        }
        public int Id { get; set; }
        public string DepotName { get; set; }
        public string SaleGroupCd { get; set; }
        public string SaleGroup { get; set; }
        public string TerritroyCd { get; set; }
        public string Territroy { get; set; }
        public string ZoneCd { get; set; }
        public string Zone { get; set; }
        public int PainterCatId { get; set; }
        public string PainterCat { get; set; }
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
        public int AttachedDealerCd { get; set; }
        public string AttachedDealer { get; set; }
        public bool IsAppInstalled { get; set; }
        public string Remark { get; set; }
        public decimal AvgMonthlyVal { get; set; }
        public float Loyality { get; set; }
        public List<string> Attachments { get; set; }


    }
}
