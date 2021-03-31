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

        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public int Division { get; set; }//
        public string BusinessArea { get; set; }
        public string SalesOffice { get; set; }//
        public string SalesGroup { get; set; }//
        public string Territory { get; set; }
        public string CustZone { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerInfoStatusLog, DealerInfoStatusLogModel>()
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.CreatedTime.ToString()))
                .ForMember(dest => dest.CustomerNo,
                    opt => opt.MapFrom(src => src.DealerInfo.CustomerNo))
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.DealerInfo.CustomerName))
                .ForMember(dest => dest.Division,
                    opt => opt.MapFrom(src => src.DealerInfo.Division))
                .ForMember(dest => dest.BusinessArea,
                    opt => opt.MapFrom(src => src.DealerInfo.BusinessArea))
                .ForMember(dest => dest.CustZone,
                    opt => opt.MapFrom(src => src.DealerInfo.CustZone))
                .ForMember(dest => dest.Territory,
                    opt => opt.MapFrom(src => src.DealerInfo.Territory))
                .ForMember(dest => dest.SalesOffice,
                    opt => opt.MapFrom(src => src.DealerInfo.SalesOffice))
                .ForMember(dest => dest.SalesGroup,
                    opt => opt.MapFrom(src => src.DealerInfo.SalesGroup));
        }
    }
}
