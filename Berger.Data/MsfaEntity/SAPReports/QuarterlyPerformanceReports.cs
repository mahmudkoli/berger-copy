using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPReports
{
    public class QuarterlyPerformanceReport
    {
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SyncTime { get; set; }
        [MaxLength(50)]
        public string Depot { get; set; }
        [MaxLength(50)]
        public string SalesGroup { get; set; }
        [MaxLength(50)]
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int NoOfBillingDealer { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MTSValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EnamelVolume { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumVolume { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecorativeValue { get; set; }
    }
}
