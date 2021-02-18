using AutoMapper;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DemandGeneration
{
    public class LeadGenerationModel : IMapFrom<LeadGeneration>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserInfoModel User { get; set; }
        public string UserFullName { get; set; }
        public string Code { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int TypeOfClientId { get; set; }
        public DropdownModel TypeOfClient { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        public int PaintingStageId { get; set; }
        public DropdownModel PaintingStage { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime ExpectedDateOfPainting { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftInteriorChangeCount { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int TotalPaintingAreaSqftExteriorChangeCount { get; set; }
        public decimal ExpectedValue { get; set; }
        public int ExpectedValueChangeCount { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        public int ExpectedMonthlyBusinessValueChangeCount { get; set; }
        public bool RequirementOfColorScheme { get; set; }
        public bool ProductSamplingRequired { get; set; }
        public DateTime NextFollowUpDate { get; set; }
        public string Remarks { get; set; }
        public string PhotoCaptureUrl { get; set; }

        public IList<LeadFollowUpModel> LeadFollowUps { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadGeneration, LeadGenerationModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty));
            profile.CreateMap<LeadGenerationModel, LeadGeneration>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }
    
    public class AppLeadGenerationModel : IMapFrom<LeadGeneration>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public DateTime LastVisitedDate { get; set; }
        public DateTime NextVisitDatePlan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadGeneration, AppLeadGenerationModel>();
        }
    }
    
    public class SaveLeadGenerationModel : IMapFrom<LeadGeneration>
    {
        //public int Id { get; set; }
        public int UserId { get; set; }
        //public UserInfo User { get; set; }
        //public string Code { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int TypeOfClientId { get; set; }
        //public DropdownDetail TypeOfClient { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        public int PaintingStageId { get; set; }
        //public DropdownDetail PaintingStage { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime ExpectedDateOfPainting { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftInteriorChangeCount { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int TotalPaintingAreaSqftExteriorChangeCount { get; set; }
        public decimal ExpectedValue { get; set; }
        public int ExpectedValueChangeCount { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        public int ExpectedMonthlyBusinessValueChangeCount { get; set; }
        public bool RequirementOfColorScheme { get; set; }
        public bool ProductSamplingRequired { get; set; }
        public DateTime NextFollowUpDate { get; set; }
        public string Remarks { get; set; }
        public string PhotoCaptureUrl { get; set; }

        //public IList<LeadFollowUpModel> LeadFollowUps { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadGeneration, SaveLeadGenerationModel>();
            profile.CreateMap<SaveLeadGenerationModel, LeadGeneration>();
        }
    }
}
