using AutoMapper;
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
        public LeadGenerationModel LeadGeneration { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public DateTime LastVisitedDate { get; set; }
        public DateTime NextVisitDatePlan { get; set; }
        public DateTime ActualVisitDate { get; set; }
        public int TypeOfClientId { get; set; }
        public DropdownDetail TypeOfClient { get; set; }
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
        public DropdownDetail ProjectStatus { get; set; }
        public int? ProjectStatusLeadCompletedId { get; set; }
        public DropdownDetail ProjectStatusLeadCompleted { get; set; }
        public int? ProjectStatusTotalLossId { get; set; }
        public DropdownDetail ProjectStatusTotalLoss { get; set; }
        public string ProjectStatusTotalLossRemarks { get; set; }
        public int? ProjectStatusPartialBusinessId { get; set; }
        public DropdownDetail ProjectStatusPartialBusiness { get; set; }
        public int ProjectStatusPartialBusinessPercentage { get; set; }
        public int SwappingCompetitionId { get; set; }
        public DropdownDetail SwappingCompetition { get; set; }
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
            profile.CreateMap<LeadFollowUp, LeadFollowUpModel>();
            profile.CreateMap<LeadFollowUpModel, LeadFollowUp>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
            profile.CreateMap<LeadBusinessAchievementModel, LeadBusinessAchievement>();
            profile.CreateMap<LeadBusinessAchievementModel, LeadBusinessAchievement>();
        }
    }

    public class SaveLeadFollowUpModel : IMapFrom<LeadFollowUp>
    {
        //public int Id { get; set; }
        public int LeadGenerationId { get; set; }
        //public LeadGenerationModel LeadGeneration { get; set; }
        //public string Depot { get; set; }
        //public string Territory { get; set; }
        //public string Zone { get; set; }
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
        public string ExpectedValueChangeReason { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
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
        public string TotalPaintingAreaSqftInteriorChangeReason { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
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
        public LeadBusinessAchievementModel BusinessAchievement { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, LeadFollowUpModel>();
            profile.CreateMap<LeadFollowUpModel, LeadFollowUp>();
            profile.CreateMap<LeadBusinessAchievement, LeadBusinessAchievementModel>();
        }
    }
}
