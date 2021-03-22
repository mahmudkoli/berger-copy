using AutoMapper;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Brand
{
    public class BrandInfoStatusLogModel : IMapFrom<BrandInfoStatusLog>
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }

        public string PropertyValue { get; set; }
        public string PropertyName { get; set; }

        public string MaterialGroupOrBrand { get; set; }
        public string PackSize { get; set; }
        public string MaterialCode { get; set; }
        public string Division { get; set; }
        public string MaterialDescription { get; set; }

        //  public BrandInfo BrandInfo { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<BrandInfoStatusLog, BrandInfoStatusLogModel>()
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.CreatedTime.ToLongDateString()))
                .ForMember(dest => dest.MaterialGroupOrBrand,
                    opt => opt.MapFrom(src => src.BrandInfo.MaterialGroupOrBrand))
                .ForMember(dest => dest.PackSize,
                    opt => opt.MapFrom(src => src.BrandInfo.PackSize))
                .ForMember(dest => dest.MaterialCode,
                    opt => opt.MapFrom(src => src.BrandInfo.MaterialCode))
                .ForMember(dest => dest.Division,
                    opt => opt.MapFrom(src => src.BrandInfo.Division))
                .ForMember(dest => dest.MaterialDescription,
                    opt => opt.MapFrom(src => src.BrandInfo.MaterialDescription));


        }
    }
}
