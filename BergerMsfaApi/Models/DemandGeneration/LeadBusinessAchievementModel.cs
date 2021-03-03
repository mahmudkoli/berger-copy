using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.DemandGeneration;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DemandGeneration
{
    public class LeadBusinessAchievementModel : IMapFrom<LeadBusinessAchievement>
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
        public string NextVisitDateText { get; set; }
        public string RemarksOrOutcome { get; set; }
        public string PhotoCaptureUrl { get; set; }

        public LeadBusinessAchievementModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadBusinessAchievement, LeadBusinessAchievementModel>()
                .AddTransform<string>(s => string.IsNullOrEmpty(s) ? string.Empty : s)
                .ForMember(dest => dest.NextVisitDateText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.NextVisitDate)));
        }
    }

    public class SaveLeadBusinessAchievementModel : IMapFrom<LeadBusinessAchievement>
    {
        public decimal BergerValueSales { get; set; }
        public decimal BergerPremiumBrandSalesValue { get; set; }
        public decimal CompetitionValueSales { get; set; }
        public string ProductSourcing { get; set; }
        public bool IsColorSchemeGiven { get; set; }
        public bool IsProductSampling { get; set; }
        public string ProductSamplingBrandName { get; set; }
        public string NextVisitDate { get; set; }
        public string RemarksOrOutcome { get; set; }
        public string PhotoCaptureUrl { get; set; }

        public SaveLeadBusinessAchievementModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadBusinessAchievement, SaveLeadBusinessAchievementModel>()
                .AddTransform<string>(s => string.IsNullOrEmpty(s) ? string.Empty : s)
                .ForMember(dest => dest.NextVisitDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.NextVisitDate)));

            profile.CreateMap<SaveLeadBusinessAchievementModel, LeadBusinessAchievement>()
                .ForMember(dest => dest.NextVisitDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateTime(src.NextVisitDate)));
        }
    }
}
