using AutoMapper;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerInfoStatusLogModel : IMapFrom<DealerInfoStatusLog>
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyName { get; set; }

        public int Division { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerInfoStatusLog, DealerInfoStatusLogModel>()
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.CreatedTime.ToLongDateString()))
                .ForMember(dest => dest.Division,
                    opt => opt.MapFrom(src => src.DealerInfo.Division))
                .ForMember(dest => dest.SalesOffice,
                    opt => opt.MapFrom(src => src.DealerInfo.SalesOffice))
                .ForMember(dest => dest.SalesGroup,
                    opt => opt.MapFrom(src => src.DealerInfo.SalesGroup));
        }
    }
}
