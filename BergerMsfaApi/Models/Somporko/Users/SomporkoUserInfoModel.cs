using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Mappings;

namespace BergerMsfaApi.Models.Somporko.Users
{
    public class SomporkoUserInfoModel : IMapFrom<UserInfo>
    {
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public int Role { get; set; }
        public string Designation { get; set; }
        public List<string> Depots { get; set; }
        public List<string> Territories { get; set; }
        public List<string> Zones { get; set; }
        public string Status { get; set; }

        public SomporkoUserInfoModel()
        {
            Depots = new List<string>();
            Territories = new List<string>();
            Zones = new List<string>();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserInfo, SomporkoUserInfoModel>()
                .ForMember(src => src.Role, opt => opt.MapFrom(dest => (int)dest.EmployeeRole))
                .ForMember(src => src.Status, opt => opt.MapFrom(dest => dest.Status == Berger.Common.Enumerations.Status.InActive ? "Inactive" : "Active"))
                .AfterMap((src, dest) => dest.ModelToList(src, dest));
        }

        public void ModelToList(UserInfo src, SomporkoUserInfoModel dest)
        {
            dest.Depots = src.UserZoneAreaMappings.Select(x => x.PlantId).Distinct().ToList();
            dest.Territories = src.UserZoneAreaMappings.Select(x => x.TerritoryId).Distinct().ToList();
            dest.Zones = src.UserZoneAreaMappings.Select(x => x.ZoneId).Distinct().ToList();
        }
    }
}


