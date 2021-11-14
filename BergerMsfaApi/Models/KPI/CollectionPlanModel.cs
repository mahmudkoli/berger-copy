using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.KPI;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.KPI
{
    public class CollectionPlanModel : IMapFrom<CollectionPlan>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public UserInfoModel User { get; set; }
        public string UserFullName { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string YearMonthText { get; set; }
        public decimal SlippageAmount { get; set; }
        public decimal CollectionTargetAmount { get; set; }
        public decimal CollectionActualAmount { get; set; }
        public decimal SlippageCollectionActualAmount { get; set; }
        public int ChangeableMaxDateDay { get; set; }
        public DateTime ChangeableMaxDate { get; set; }
        public string ChangeableMaxDateText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CollectionPlan, CollectionPlanModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.YearMonthText,
                    opt => opt.MapFrom(src => new DateTime(src.Year > 0 ? src.Year : 1, src.Month > 0 ? src.Month : 1, 01).ToString("yyyy MMM")));

            profile.CreateMap<CollectionPlanModel, CollectionPlan>();
        }
    }

    public class SaveCollectionPlanModel : IMapFrom<CollectionPlan>
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        //public int Year { get; set; }
        //public int Month { get; set; }
        public decimal SlippageAmount { get; set; }
        public decimal CollectionTargetAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CollectionPlan, SaveCollectionPlanModel>();
            profile.CreateMap<SaveCollectionPlanModel, CollectionPlan>();
        }
    }

    public class CustomerSlippageQueryModel
    {
        public string BusinessArea { get; set; }
        public string Territory { get; set; }
    }

    public class CollectionPlanQueryObjectModel : QueryObjectModel
    {
        public string BusinessArea { get; set; }
        public IList<string> Territories { get; set; }
        public CollectionPlanQueryObjectModel()
        {
            this.Territories = new List<string>();
        }
    }
}
