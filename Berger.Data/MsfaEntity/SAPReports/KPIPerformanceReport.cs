using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.SAPReports
{
    public class KPIPerformanceReport
    {
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime SyncTime { get; set; }
        [MaxLength(50)]
        public string Depot { get; set; }
        [MaxLength(50)]
        public string SalesGroup { get; set; }
        [MaxLength(50)]
        public string Territory { get; set; }
        [MaxLength(50)]
        public string Division { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }

        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Volume { get; set; }

        [MaxLength(50)]
        public string CustomerNo { get; set; }
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [MaxLength(50)]
        public string CustomerClassification { get; set; }
    }
}