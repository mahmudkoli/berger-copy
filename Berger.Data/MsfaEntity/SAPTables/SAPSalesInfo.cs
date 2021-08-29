using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPTables
{
    [Table("SAPSalesInfos")]
    public class SAPSalesInfo
    {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [Column("bukrs")]
        [MaxLength(50)]
        public string CompanyCode { get; set; } //................................
        [Column("spart")]
        [MaxLength(50)]
        public string Division { get; set; }
        [Column("matkl")]
        [MaxLength(50)]
        public string MatarialGroupOrBrand { get; set; }
        [Column("wgbez")]
        [MaxLength(200)]
        public string MatarialGroupOrBrandName { get; set; }
        [Column("matnr")]
        [MaxLength(50)]
        public string MatrialCode { get; set; }
        [Column("vkorg")]
        [MaxLength(50)]
        public string SalesOrgranization { get; set; } //............
        [Column("kunrg")]
        [MaxLength(50)]
        public string CustomerNoOrSoldToParty { get; set; }
        [Column("kunnr_sh")]
        [MaxLength(50)]
        public string CustomerNoOrShipToParty { get; set; } //..............................
        [Column("Payer_DL")]
        [MaxLength(50)]
        public string CustomerNo { get; set; }
        [Column("vbeln")]
        [MaxLength(200)]
        public string InvoiceNoOrBillNo { get; set; }
        [Column("vkbur_c")]
        [MaxLength(50)]
        public string SalesOffice { get; set; }
        [Column("vkgrp_c")]
        [MaxLength(50)]
        public string SalesGroup { get; set; }
        [Column("kukla")]
        [MaxLength(50)]
        public string CustomerClassification { get; set; }
        [Column("fkdat")]
        [MaxLength(50)]
        public string Date { get; set; }
        [Column("posnr")]
        [MaxLength(50)]
        public string LineNumber { get; set; }
        [Column("arktx")]
        [MaxLength(200)]
        public string MatarialDescription { get; set; }
        [Column("meins")]
        [MaxLength(50)]
        public string UnitOfMeasure { get; set; }
        [Column("voleh")]
        [MaxLength(50)]
        public string VolumeUnit { get; set; } //....................................
        [Column("Territory")]
        [MaxLength(50)]
        public string Territory { get; set; }
        [Column("Szone")]
        [MaxLength(50)]
        public string Zone { get; set; }
        [Column("cname")]
        [MaxLength(200)]
        public string CustomerName { get; set; }
        [Column("spart_text")]
        [MaxLength(200)]
        public string DivisionName { get; set; }
        [Column("Revenue")]
        [MaxLength(200)]
        public string NetAmount { get; set; }
        [Column("gsber")]
        [MaxLength(50)]
        public string PlantOrBusinessArea { get; set; }
        [Column("fkimg")]
        [MaxLength(50)]
        public string Quantity { get; set; }
        [Column("volum")]
        [MaxLength(200)]
        public string Volume { get; set; }
        [Column("ktokd")]
        [MaxLength(50)]
        public string CustomerAccountGroup { get; set; }
        [Column("vtweg")]
        [MaxLength(50)]
        public string DistributionChannel { get; set; }
        [Column("erzet_T")]
        [MaxLength(50)]
        public string Time { get; set; }
        [Column("kkber")]
        [MaxLength(50)]
        public string CreditControlArea { get; set; }
    }
}
