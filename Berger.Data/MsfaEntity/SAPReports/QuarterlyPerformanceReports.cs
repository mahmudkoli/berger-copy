using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPReports
{
    public class QuarterlyPerformanceReport
    {
        public int Id { get; set; }
        public DateTime SyncTime { get; set; }
        public string Depot { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int NoOfBillingDealer { get; set; }
        public decimal MTSValue { get; set; }
        public decimal EnamelVolume { get; set; }
        public decimal PremiumValue { get; set; }
        public decimal PremiumVolume { get; set; }
        public decimal DecorativeValue { get; set; }
    }
}
