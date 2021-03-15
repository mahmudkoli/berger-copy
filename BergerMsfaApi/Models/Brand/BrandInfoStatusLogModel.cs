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
        public BrandInfo BrandInfo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BrandInfoStatusLog, BrandInfoStatusLogModel>()
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.CreatedTime.ToLongDateString()));


        }
    }
}
