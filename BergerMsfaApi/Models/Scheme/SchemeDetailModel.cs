using System;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Mappings;

namespace BergerMsfaApi.Models.Scheme
{
    public class SchemeDetailModel : IMapFrom<SchemeDetail>
    {
        public int Id { get; set; }
        //National Scheme (Brand)
        public string Code { get; set; }
        public string Brand { get; set; }
        public string RateInLtrOrKg { get; set; }
        public string RateInSKU { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        //public string BenefitDate { get; set; }



        //Common
        public DateTime BenefitStartDate { get; set; }
        public DateTime? BenefitEndDate { get; set; }
        public string BenefitStartDateText { get; set; }
        public string BenefitEndDateText { get; set; }

        public int SchemeMasterId { get; set; }
        public string SchemeMasterName { get; set; }
        public string SchemeMasterCondition { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeDetail, SchemeDetailModel>()
                .ForMember(dest => dest.SchemeMasterName,
                    opt => opt.MapFrom(src => src.SchemeMaster != null ? $"{src.SchemeMaster.SchemeName}" : string.Empty))
                .ForMember(dest => dest.SchemeMasterCondition,
                    opt => opt.MapFrom(src => src.SchemeMaster != null ? $"{src.SchemeMaster.Condition}" : string.Empty));

            profile.CreateMap<SchemeDetailModel, SchemeDetail>();
        }
    }

    public class SaveSchemeDetailModel : IMapFrom<SchemeDetail>
    {
        public int Id { get; set; }
        //National Scheme (Brand)
        public string Code { get; set; }
        public string Brand { get; set; }
        public string RateInLtrOrKg { get; set; }
        public string RateInSKU { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        public string BenefitDate { get; set; }


        public int SchemeMasterId { get; set; }
        public Status Status { get; set; }
        public DateTime BenefitStartDate { get; set; }
        public DateTime? BenefitEndDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeDetail, SaveSchemeDetailModel>();
            profile.CreateMap<SaveSchemeDetailModel, SchemeDetail>();
        }
    }

    public class AppSchemeDetailModel : IMapFrom<SchemeDetail>
    {
        public int Id { get; set; }
        //National Scheme (Brand)
        //public string Code { get; set; }
        public string Brand { get; set; }
        public string RateInLtrOrKg { get; set; }
        public string RateInSKU { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        //public string BenefitDate { get; set; }


        //Common
        public string BenefitStartDate { get; set; }
        public string BenefitEndDate { get; set; }

        public int SchemeMasterId { get; set; }
        public string SchemeMasterName { get; set; }
        public string SchemeMasterCondition { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeDetail, AppSchemeDetailModel>()
                .ForMember(dest => dest.SchemeMasterName,
                    opt => opt.MapFrom(src => src.SchemeMaster != null ? $"{src.SchemeMaster.SchemeName}" : string.Empty))
                .ForMember(dest => dest.SchemeMasterCondition,
                    opt => opt.MapFrom(src => src.SchemeMaster != null ? $"{src.SchemeMaster.Condition}" : string.Empty))
                .ForMember(dest => dest.BenefitStartDate,
                    opt => opt.MapFrom(src => src.BenefitStartDate != null ? $"{src.BenefitStartDate.ToString("yyyy-MM-dd")}" : string.Empty))
                .ForMember(dest => dest.BenefitEndDate,
                    opt => opt.MapFrom(src => src.BenefitEndDate != null ? $"{src.BenefitEndDate.Value.ToString("yyyy-MM-dd")}" : string.Empty));

            profile.CreateMap<AppSchemeDetailModel, SchemeDetail>();
        }
    }
}
