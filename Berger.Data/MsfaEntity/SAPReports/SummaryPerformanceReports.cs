using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.SAPReports
{
    public class SummaryPerformanceReport
    {
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SyncTime { get; set; }
        [MaxLength(50)]
        public string Depot { get; set; }
        [MaxLength(50)]
        public string SalesGroup { get; set; }
        [MaxLength(50)]
        public string Territory { get; set; }
        [MaxLength(50)]
        public string Zone { get; set; }
        [MaxLength(50)]
        public string Division { get; set; }
        [MaxLength(50)]
        public string CreditControlArea { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Volume { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillDateValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillDateVolume { get; set; }
    }
}
