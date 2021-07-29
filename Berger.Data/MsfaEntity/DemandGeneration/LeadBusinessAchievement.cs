using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DemandGeneration
{
    public class LeadBusinessAchievement : Entity<int>
    {
        public decimal BergerValueSales { get; set; }
        public decimal BergerPremiumBrandSalesValue { get; set; }
        public decimal CompetitionValueSales { get; set; }
        //public string ProductSourcing { get; set; }
        public int ProductSourcingId { get; set; }
        public DropdownDetail ProductSourcing { get; set; }
        public string ProductSourcingRemarks { get; set; } // multiple dealer id and name separated by comma
        public bool IsColorSchemeGiven { get; set; }
        public bool IsProductSampling { get; set; }
        public string ProductSamplingBrandName { get; set; } // multiple brand name separated by comma
        public DateTime NextVisitDate { get; set; }
        public string RemarksOrOutcome { get; set; }
        public string PhotoCaptureUrl { get; set; }
    }
}
