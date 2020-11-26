using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DemandGeneration
{
    public class LeadBusinessAchievementModel
    {
        public int Id { get; set; }
        public decimal BergerValueSales { get; set; }
        public decimal BergerPremiumBrandSalesValue { get; set; }
        public decimal CompetitionValueSales { get; set; }
        public string ProductSourcing { get; set; }
        public bool IsColorSchemeGiven { get; set; }
        public bool IsProductSampling { get; set; }
        public string ProductSamplingBrandName { get; set; }
        public DateTime NextVisitDate { get; set; }
        public string RemarksOrOutcome { get; set; }
        public string PhotoCaptureUrl { get; set; }
    }
}
