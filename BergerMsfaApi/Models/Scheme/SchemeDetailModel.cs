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
        public string RateInDrum { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        public string BenefitDate { get; set; }

        //Painter Scheme
        public string SchemeId { get; set; }
        public string Material { get; set; }
        public string TargetVolume { get; set; }

        //Common
        public string Benefit { get; set; }

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
        public string RateInDrum { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        public string BenefitDate { get; set; }

        //Painter Scheme
        public string SchemeId { get; set; }
        public string Material { get; set; }
        public string TargetVolume { get; set; }

        //Common
        public string Benefit { get; set; }

        public int SchemeMasterId { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeDetail, SaveSchemeDetailModel>();
            profile.CreateMap<SaveSchemeDetailModel, SchemeDetail>();
        }
    }
}
