using Berger.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Report
{
    public class StrikeRateKPIReportResultModel
    {
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        public string Date { get; set; }
        public int NoOfCallActual { get; set; }
        public int NoOfPremiumBrandBilling { get; set; }
        public decimal BillingPercentage { get; set; }

        public StrikeRateKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
