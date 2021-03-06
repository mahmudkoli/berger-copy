using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Mappings;

namespace BergerMsfaApi.Models.Users
{
    public class UserInfoModel : IMapFrom<UserInfo>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //public string Code { get; set; }
        public string EmployeeId { get; set; }
        public EnumEmployeeRole EmployeeRole { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }

        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }

        public List<int> RoleIds { get; set; }
        public List<string> PlantIds { get; set; }
        public List<string> AreaIds { get; set; }
        public List<string> ZoneIds { get; set; }
        public List<string> SaleOfficeIds { get; set; }
        public List<string> TerritoryIds { get; set; }
        public Status Status { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNames { get; set; }

        public UserInfoModel()
        {
            RoleIds = new List<int>();
            PlantIds = new List<string>();
            AreaIds = new List<string>();
            ZoneIds = new List<string>();
            SaleOfficeIds = new List<string>();
            TerritoryIds = new List<string>();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserInfoModel, UserInfo>();
            profile.CreateMap<UserInfo, UserInfoModel>()
                .ForMember(src => src.RoleId, opt => opt.MapFrom(dest => dest.Roles.Any() ? dest.Roles.FirstOrDefault().RoleId : 0))
                .ForMember(src => src.RoleName, opt => opt.MapFrom(dest => dest.Roles.Any() ? dest.Roles.FirstOrDefault().Role.Name : ""))
                .ForMember(src => src.RoleNames, opt => opt.MapFrom(dest => dest.Roles.Any() ? 
                                                        string.Join(", ", dest.Roles.Select(x => x.Role.Name)) : ""));
        }
    }

    public class SaveUserInfoModel : IMapFrom<UserInfo>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //public string Code { get; set; }
        public string EmployeeId { get; set; }
        public EnumEmployeeRole EmployeeRole { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }

        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }

        public List<int> RoleIds { get; set; }
        public List<string> PlantIds { get; set; }
        public List<string> AreaIds { get; set; }
        public List<string> ZoneIds { get; set; }
        public List<string> SaleOfficeIds { get; set; }
        public List<string> TerritoryIds { get; set; }
        public Status Status { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public SaveUserInfoModel()
        {
            RoleIds = new List<int>();
            PlantIds = new List<string>();
            AreaIds = new List<string>();
            ZoneIds = new List<string>();
            SaleOfficeIds = new List<string>();
            TerritoryIds = new List<string>();
        }
    }
}


