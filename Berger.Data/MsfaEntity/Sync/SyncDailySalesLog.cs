using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Sync
{
    public class SyncDailySalesLog : AuditableEntity<int>
    {
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [StringLength(150)]
        public string Zone { get; set; }
        [StringLength(20)]
        public string TerritoryCode { get; set; }
        [StringLength(150)]
        public string TerritoryName { get; set; }

        [StringLength(20)]
        public string BusinessArea { get; set; }

        [StringLength(250)]
        public string BusinessAreaName { get; set; }

        [StringLength(20)]
        public string CreditControlArea { get; set; } 
        
        [StringLength(250)]
        public string CreditControlAreaName { get; set; }

        [StringLength(20)]
        public string SalesGroup { get; set; } 

        [StringLength(250)]
        public string SalesGroupName { get; set; }
        
        [StringLength(250)]
        public string SalesOfficeName { get; set; }
        
        [StringLength(20)]
        public string SalesOffice { get; set; }
        
        [StringLength(20)]
        public string AccountGroup { get; set; } 
        
        [StringLength(20)]
        public string BrandCode { get; set; }

        [StringLength(20)]
        public string Division { get; set; }
        
        [StringLength(250)]
        public string DivisionName { get; set; }

        public int CustNo { get; set; }
        public double Volume { get; set; }
        public double NetAmount { get; set; }
    }
}
