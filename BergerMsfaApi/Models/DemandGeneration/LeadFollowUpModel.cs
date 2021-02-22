using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DemandGeneration
{
    public class LeadFollowUpModel : IMapFrom<LeadFollowUp>
    {
        public int Id { get; set; }
        public int LeadGenerationId { get; set; }
        //public LeadGenerationModel LeadGeneration { get; set; }
        //public string Depot { get; set; }
        //public string Territory { get; set; }
        //public string Zone { get; set; }
        public DateTime LastVisitedDate { get; set; }
        public string LastVisitedDateText { get; set; }
        public DateTime NextVisitDatePlan { get; set; }
        public string NextVisitDatePlanText { get; set; }
        public DateTime ActualVisitDate { get; set; }
        public string ActualVisitDateText { get; set; }
        public int TypeOfClientId { get; set; }
        //public DropdownDetail TypeOfClient { get; set; }
        public string TypeOfClientText { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonNameChangeReason { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string KeyContactPersonMobileChangeReason { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorNameChangeReason { get; set; }
        public string PaintContractorMobile { get; set; }
        public string PaintContractorMobileChangeReason { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public string NumberOfStoriedBuildingChangeReason { get; set; }
        public decimal ExpectedValue { get; set; }
        //public int ExpectedValueChangeCount { get; set; }
        public string ExpectedValueChangeReason { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        //public int ExpectedMonthlyBusinessValueChangeCount { get; set; }
        public string ExpectedMonthlyBusinessValueChangeReason { get; set; }
        public int ProjectStatusId { get; set; }
        //public DropdownDetail ProjectStatus { get; set; }
        public string ProjectStatusText { get; set; }
        public int? ProjectStatusLeadCompletedId { get; set; }
        //public DropdownDetail ProjectStatusLeadCompleted { get; set; }
        public string ProjectStatusLeadCompletedText { get; set; }
        public int? ProjectStatusTotalLossId { get; set; }
        //public DropdownDetail ProjectStatusTotalLoss { get; set; }
        public string ProjectStatusTotalLossText { get; set; }
        public string ProjectStatusTotalLossRemarks { get; set; }
        public int? ProjectStatusPartialBusinessId { get; set; }
        //public DropdownDetail ProjectStatusPartialBusiness { get; set; }
        public string ProjectStatusPartialBusinessText { get; set; }
        public int ProjectStatusPartialBusinessPercentage { get; set; }
        public int SwappingCompetitionId { get; set; }
        //public DropdownDetail SwappingCompetition { get; set; }
        public string SwappingCompetitionText { get; set; }
        public string SwappingCompetitionAnotherCompetitorName { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftInteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftInteriorChangeReason { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int TotalPaintingAreaSqftExteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftExteriorChangeReason { get; set; }
        public string UpTradingFromBrandName { get; set; }
        public string UpTradingToBrandName { get; set; }
        public string BrandUsedInteriorBrandName { get; set; }
        public string BrandUsedExteriorBrandName { get; set; }
        public string BrandUsedUnderCoatBrandName { get; set; }
        public string BrandUsedTopCoatBrandName { get; set; }
        public int ActualPaintJobCompletedInteriorPercentage { get; set; }
        public int ActualPaintJobCompletedExteriorPercentage { get; set; }
        public int ActualVolumeSoldInteriorGallon { get; set; }
        public int ActualVolumeSoldInteriorKg { get; set; }
        public int ActualVolumeSoldExteriorGallon { get; set; }
        public int ActualVolumeSoldExteriorKg { get; set; }
        public int ActualVolumeSoldUnderCoatGallon { get; set; }
        public int ActualVolumeSoldTopCoatGallon { get; set; }
        public int BusinessAchievementId { get; set; }
        public LeadBusinessAchievementModel BusinessAchievement { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, LeadFollowUpModel>()
                .ForMember(dest => dest.TypeOfClientText,
                    opt => opt.MapFrom(src => src.TypeOfClient != null ? $"{src.TypeOfClient.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusText,
                    opt => opt.MapFrom(src => src.ProjectStatus != null ? $"{src.ProjectStatus.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusLeadCompletedText,
                    opt => opt.MapFrom(src => src.ProjectStatusLeadCompleted != null ? $"{src.ProjectStatusLeadCompleted.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusTotalLossText,
                    opt => opt.MapFrom(src => src.ProjectStatusTotalLoss != null ? $"{src.ProjectStatusTotalLoss.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusPartialBusinessText,
                    opt => opt.MapFrom(src => src.ProjectStatusPartialBusiness != null ? $"{src.ProjectStatusPartialBusiness.DropdownName}" : string.Empty))
                .ForMember(dest => dest.SwappingCompetitionText,
                    opt => opt.MapFrom(src => src.SwappingCompetition != null ? $"{src.SwappingCompetition.DropdownName}" : string.Empty))
                .ForMember(dest => dest.LastVisitedDateText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.LastVisitedDate)))
                .ForMember(dest => dest.NextVisitDatePlanText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.NextVisitDatePlan)))
                .ForMember(dest => dest.ActualVisitDateText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.ActualVisitDate)));
            //profile.CreateMap<LeadFollowUpModel, LeadFollowUp>();
            //profile.CreateMap<DropdownDetail, DropdownModel>();
            //profile.CreateMap<DropdownModel, DropdownDetail>();
            profile.CreateMap<LeadBusinessAchievement, LeadBusinessAchievementModel>();
            //profile.CreateMap<LeadBusinessAchievementModel, LeadBusinessAchievement>();
        }
    }

    public class SaveLeadFollowUpModel : IMapFrom<LeadFollowUp>
    {
        //public int Id { get; set; }
        public int LeadGenerationId { get; set; }
        //public LeadGenerationModel LeadGeneration { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public DateTime LastVisitedDate { get; set; }
        public DateTime NextVisitDatePlan { get; set; }
        public DateTime ActualVisitDate { get; set; }
        public int TypeOfClientId { get; set; }
        //public DropdownDetail TypeOfClient { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonNameChangeReason { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string KeyContactPersonMobileChangeReason { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorNameChangeReason { get; set; }
        public string PaintContractorMobile { get; set; }
        public string PaintContractorMobileChangeReason { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public string NumberOfStoriedBuildingChangeReason { get; set; }
        public decimal ExpectedValue { get; set; }
        public int ExpectedValueChangeCount { get; set; }
        public string ExpectedValueChangeReason { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        public int ExpectedMonthlyBusinessValueChangeCount { get; set; }
        public string ExpectedMonthlyBusinessValueChangeReason { get; set; }
        public int ProjectStatusId { get; set; }
        //public DropdownDetail ProjectStatus { get; set; }
        public int? ProjectStatusLeadCompletedId { get; set; }
        //public DropdownDetail ProjectStatusLeadCompleted { get; set; }
        public int? ProjectStatusTotalLossId { get; set; }
        //public DropdownDetail ProjectStatusTotalLoss { get; set; }
        public string ProjectStatusTotalLossRemarks { get; set; }
        public int? ProjectStatusPartialBusinessId { get; set; }
        //public DropdownDetail ProjectStatusPartialBusiness { get; set; }
        public int ProjectStatusPartialBusinessPercentage { get; set; }
        public int SwappingCompetitionId { get; set; }
        //public DropdownDetail SwappingCompetition { get; set; }
        public string SwappingCompetitionAnotherCompetitorName { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftInteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftInteriorChangeReason { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int TotalPaintingAreaSqftExteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftExteriorChangeReason { get; set; }
        public string UpTradingFromBrandName { get; set; }
        public string UpTradingToBrandName { get; set; }
        public string BrandUsedInteriorBrandName { get; set; }
        public string BrandUsedExteriorBrandName { get; set; }
        public string BrandUsedUnderCoatBrandName { get; set; }
        public string BrandUsedTopCoatBrandName { get; set; }
        public int ActualPaintJobCompletedInteriorPercentage { get; set; }
        public int ActualPaintJobCompletedExteriorPercentage { get; set; }
        public int ActualVolumeSoldInteriorGallon { get; set; }
        public int ActualVolumeSoldInteriorKg { get; set; }
        public int ActualVolumeSoldExteriorGallon { get; set; }
        public int ActualVolumeSoldExteriorKg { get; set; }
        public int ActualVolumeSoldUnderCoatGallon { get; set; }
        public int ActualVolumeSoldTopCoatGallon { get; set; }
        //public int BusinessAchievementId { get; set; }
        public SaveLeadBusinessAchievementModel BusinessAchievement { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, SaveLeadFollowUpModel>();
            profile.CreateMap<SaveLeadFollowUpModel, LeadFollowUp>();
            profile.CreateMap<LeadBusinessAchievement, SaveLeadBusinessAchievementModel>();
            profile.CreateMap<SaveLeadBusinessAchievementModel, LeadBusinessAchievement>();
        }
    }
}
