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

    public class BusinessCallKPIReportResultModel
    {
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        public string Date { get; set; }
        public int NoOfCallTarget { get; set; }
        public int NoOfCallActual { get; set; }
        public decimal Achivement { get; set; }
        public int ExclusiveNoOfCallTarget { get; set; }
        public int ExclusiveNoOfCallActual { get; set; }
        public decimal ExclusiveAchivement { get; set; }
        public int NonExclusiveNoOfCallTarget { get; set; }
        public int NonExclusiveNoOfCallActual { get; set; }
        public decimal NonExclusiveAchivement { get; set; }

        public BusinessCallKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
