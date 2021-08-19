using Berger.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AutoMapper;
using BergerMsfaApi.Mappings;

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

    public class BusinessCallBaseKPIReportResultModel
    {
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        public int NoOfCallTarget { get; set; }
        public int NoOfCallActual { get; set; }
        public decimal Achivement { get; set; }

        public BusinessCallBaseKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class BusinessCallAPPKPIReportResultModel : BusinessCallBaseKPIReportResultModel, IMapFrom<BusinessCallWebKPIReportResultModel>
    {
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BusinessCallWebKPIReportResultModel, BusinessCallAPPKPIReportResultModel>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Date));
        }

    }

    public class BusinessCallWebKPIReportResultModel : BusinessCallBaseKPIReportResultModel
    {

        public string Date { get; set; }
        public int ExclusiveNoOfCallTarget { get; set; }
        public int ExclusiveNoOfCallActual { get; set; }
        public decimal ExclusiveAchivement { get; set; }
        public int NonExclusiveNoOfCallTarget { get; set; }
        public int NonExclusiveNoOfCallActual { get; set; }
        public decimal NonExclusiveAchivement { get; set; }

        public BusinessCallWebKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }






    public class BillingAnalysisKPIReportResultModel
    {
        //public EnumBillingAnalysisType BillingAnalysisType { get; set; }
        public string DealerType { get; set; }
        public int NoOfDealer { get; set; }
        public int NoOfBillingDealer { get; set; }
        public decimal BillingPercentage { get; set; }
        //public IList<BillingAnalysisDetailsKPIReportResultModel> Details { get; set; }

        public BillingAnalysisKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            //this.Details = new List<BillingAnalysisDetailsKPIReportResultModel>();
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
        Exclusive = 1,
        NonAPNonExclusive = 2,
        NonExclusive = 3,
        New = 4,
        Total = 5,
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

    public class ColorBankInstallationPlanVsActualKPIReportResultModel
    {
        public string Month { get; set; }
        public int Target { get; set; }
        public int Actual { get; set; }
        public decimal TargetAchievement { get; set; }
    }

    public class ColorBankProductivityBase
    {
        public decimal LYProductivity { get; set; }
        public int ProductivityTarget { get; set; }
        public decimal CYActualProductivity { get; set; }
        public decimal ProductivityGrowth { get; set; }
    }

    public class ColorBankProductivityWeb : ColorBankProductivityBase
    {
        public string Territory { get; set; }
    }

    public class CollectionPlanKPIReportResultModelForApp
    {
        public decimal ImmediateLMSlippageAmount { get; set; }
        public decimal MTDCollectionPlan { get; set; }
        public decimal MTDActualCollection { get; set; }
        public decimal TargetAch { get; set; }

        public CollectionPlanKPIReportResultModelForApp()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
