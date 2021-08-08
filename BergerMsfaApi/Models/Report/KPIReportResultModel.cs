﻿using Berger.Common.Extensions;
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
        Exclusive = 1,
        NonAPNonExclusive = 2,
        NonExclusive = 3,
        New = 4,
        Total = 5,
    }

    public class CollectionPlanKPIReportResultModel
    {
        public decimal SlippageAmount { get; set; }
        public decimal CollectionTargetAmount { get; set; }
        public decimal CollectionActualAmount { get; set; }
        public decimal CollectionActualSlippageAmount { get; set; }

        public CollectionPlanKPIReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
