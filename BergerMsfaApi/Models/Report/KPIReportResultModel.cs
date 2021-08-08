﻿using Berger.Common.Extensions;
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

    public class BillingAnalysisKPIReportResultModel
    {
        public EnumBillingAnalysisType BillingAnalysisType { get; set; }
        public string BillingAnalysisTypeText { get; set; }
        public int NoOfDealer { get; set; }
        public int NoOfBillingDealer { get; set; }
        public decimal BillingPercentage { get; set; }
        public IList<BillingAnalysisDetailsKPIReportResultModel> Details { get; set; }

        public BillingAnalysisKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.Details = new List<BillingAnalysisDetailsKPIReportResultModel>();
        }
    }

    public class BillingAnalysisDetailsKPIReportResultModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public bool IsBilling { get; set; }
        public string IsBillingText { get; set; }

        public BillingAnalysisDetailsKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public enum EnumBillingAnalysisType
    {
        Exclusive=1,
        NonAPNonExclusive=2,
        NonExclusive=3,
        New=4,
        Total=5,
    }

    public class CollectionPlanKPIReportResultModel
    {
        public string Territory { get; set; }
        public decimal ImmediateLMSlippageAmount { get; set; }
        public decimal MTDCollectionPlan { get; set; }
        public decimal MTDActualCollection { get; set; }
        public decimal TargetAch { get; set; }

        public CollectionPlanKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
