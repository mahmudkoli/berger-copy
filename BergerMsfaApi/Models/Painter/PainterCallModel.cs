using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCallModel : IMapFrom<PainterCall>
    {
        public PainterCallModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PainterCall, PainterCallModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HasSchemeComnunaction, opt => opt.MapFrom(src => src.HasSchemeComnunaction))
                .ForMember(dest => dest.HasPremiumProtBriefing, opt => opt.MapFrom(src => src.HasPremiumProtBriefing))
                .ForMember(dest => dest.HasNewProBriefing, opt => opt.MapFrom(src => src.HasNewProBriefing))
                .ForMember(dest => dest.HasUsageEftTools, opt => opt.MapFrom(src => src.HasUsageEftTools))
                .ForMember(dest => dest.HasAppUsage, opt => opt.MapFrom(src => src.HasAppUsage))
                .ForMember(dest => dest.WorkInHandNumber, opt => opt.MapFrom(src => src.WorkInHandNumber))
                .ForMember(dest => dest.HasDbblIssue, opt => opt.MapFrom(src => src.HasDbblIssue))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.PainterId, opt => opt.MapFrom(src => src.PainterId));


        }
        
        public int Id { get; set; }
        public bool HasSchemeComnunaction { get; set; }
        public bool HasPremiumProtBriefing { get; set; }
        public bool HasNewProBriefing { get; set; }
        public bool HasUsageEftTools { get; set; }
        public bool HasAppUsage { get; set; }
        public decimal WorkInHandNumber { get; set; }
        public bool HasDbblIssue { get; set; }
        public string Comment { get; set; }
        public int PainterId { get; set; }
        public List<PainterCompanyMTDValueModel> PainterCompanyMTDValue{ get; set; }


    }
}
