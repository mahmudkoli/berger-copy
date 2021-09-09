using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPReports
{
    public class CategoryWisePerformanceReport
    {
        public int Id { get; set; }
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
        public string Zone { get; set; }
        [MaxLength(50)]
        public string CustomerNo { get; set; }
        [MaxLength(200)]
        public string CustomerName { get; set; }
        [MaxLength(50)]
        public string CustomerClassification { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Volume { get; set; }
    }
}
