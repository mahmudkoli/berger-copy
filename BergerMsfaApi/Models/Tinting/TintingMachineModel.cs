using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Tinting;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Tinting
{
    public class TintingMachineModel : IMapFrom<TintingMachine>
    {
        public int Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public int UserInfoId { get; set; }
        public string UserFullName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int NoOfActiveMachine { get; set; }
        public int NoOfInactiveMachine { get; set; }
        public int No { get; set; }
        public decimal Contribution { get; set; }
        public int NoOfCorrection { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TintingMachine, TintingMachineModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.UserInfo != null ? $"{src.UserInfo.FullName}" : string.Empty))
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Company != null ? $"{src.Company.DropdownName}" : string.Empty));

            profile.CreateMap<TintingMachineModel, TintingMachine>();
        }
    }

    public class SaveTintingMachineModel : IMapFrom<TintingMachine>
    {
        public int Id { get; set; }
        public string Territory { get; set; }
        public int UserInfoId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int NoOfActiveMachine { get; set; }
        public int NoOfInactiveMachine { get; set; }
        public int No { get; set; }
        public decimal Contribution { get; set; }
        public int NoOfCorrection { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TintingMachine, SaveTintingMachineModel>()
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Company != null ? $"{src.Company.DropdownName}" : string.Empty));

            profile.CreateMap<SaveTintingMachineModel, TintingMachine>();
        }
    }

    public class AppTintingMachineModel : IMapFrom<TintingMachine>
    {
        public int Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public int UserInfoId { get; set; }
        public string UserFullName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int NoOfActiveMachine { get; set; }
        public int NoOfInactiveMachine { get; set; }
        public int No { get; set; }
        public decimal Contribution { get; set; }
        public int NoOfCorrection { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TintingMachine, AppTintingMachineModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.UserInfo != null ? $"{src.UserInfo.FullName}" : string.Empty))
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Company != null ? $"{src.Company.DropdownName}" : string.Empty));
        }
    }

    public class AppTintingMachineSearchModel
    {
        public string Depot { get; set; }
        public string Territory { get; set; }
        public int? UserId { get; set; }
    }
}
