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
        [StringLength(20)]
        public string CreditControlArea { get; set; }
        [StringLength(20)]
        public string SalesGroup { get; set; }
        public double Volume { get; set; }
        public double NetAmount { get; set; }
    }
}
