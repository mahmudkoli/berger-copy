using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Common;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerInfoModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool IsFocused { get; set; }
        public bool IsSubdealer { get; set; }
        public DateTime VisitDate { get; set; }
        public string PlanDate { get; set; }
    }

    public class DealerInfoPortalModel : IMapFrom<DealerInfo>
    {
        public int Id { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Depot { get; set; } // Plant, Depot
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; } // Area
        public string Territory { get; set; }
        public string Zone { get; set; } // Zone
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }

        public bool IsExclusive { get; set; }
        public bool IsLastYearAppointed { get; set; }
        public bool IsAP { get; set; }
        public EnumClubSupreme ClubSupremeType { get; set; }
        public EnumBussinesCategory BussinesCategoryType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerInfo, DealerInfoPortalModel>()
                .ForMember(dest => dest.Depot, opt => opt.MapFrom(src => src.BusinessArea))
                .ForMember(dest => dest.Zone, opt => opt.MapFrom(src => src.CustZone));
        }
    }

    public class DealerInfoStatusModel
    {
        public int DealerId { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }

    public class DealerInfoQueryObjectModel : QueryObjectModel
    {
        public string Depot { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }
        public int DealerId { get; set; }

        public DealerInfoQueryObjectModel()
        {
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }
}
