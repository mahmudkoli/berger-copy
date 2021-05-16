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
        public string OtherClientName { get; set; }
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
        public string ProjectStatusTotalLossRemarks { get; set; }
        public decimal ProjectStatusPartialBusinessPercentage { get; set; }
        public bool HasSwappingCompetition { get; set; }
        public int? SwappingCompetitionId { get; set; }
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
        public decimal ActualPaintJobCompletedInteriorPercentage { get; set; }
        public decimal ActualPaintJobCompletedExteriorPercentage { get; set; }
        public decimal ActualVolumeSoldInteriorGallon { get; set; }
        public decimal ActualVolumeSoldInteriorKg { get; set; }
        public decimal ActualVolumeSoldExteriorGallon { get; set; }
        public decimal ActualVolumeSoldExteriorKg { get; set; }
        public decimal ActualVolumeSoldUnderCoatGallon { get; set; }
        public decimal ActualVolumeSoldTopCoatGallon { get; set; }
        public int BusinessAchievementId { get; set; }
        public LeadBusinessAchievementModel BusinessAchievement { get; set; }

        public LeadFollowUpModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, LeadFollowUpModel>()
                .AddTransform<string>(s => string.IsNullOrEmpty(s) ? string.Empty : s)
                .ForMember(dest => dest.TypeOfClientText,
                    opt => opt.MapFrom(src => src.TypeOfClient != null ? $"{src.TypeOfClient.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusText,
                    opt => opt.MapFrom(src => src.ProjectStatus != null ? $"{src.ProjectStatus.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusLeadCompletedText,
                    opt => opt.MapFrom(src => src.ProjectStatusLeadCompleted != null ? $"{src.ProjectStatusLeadCompleted.DropdownName}" : string.Empty))
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
            //profile.CreateMap<LeadBusinessAchievement, LeadBusinessAchievementModel>();
            //profile.CreateMap<LeadBusinessAchievementModel, LeadBusinessAchievement>();
        }
    }

    public class AppSaveLeadFollowUpModel : IMapFrom<LeadFollowUp>
    {
        //public int Id { get; set; }
        public int LeadGenerationId { get; set; }
        //public LeadGenerationModel LeadGeneration { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string DepotName { get; set; }
        public string TerritoryName { get; set; }
        public string ZoneName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string LastVisitedDate { get; set; }
        public string NextVisitDatePlan { get; set; }
        public string ActualVisitDate { get; set; }
        public int? TypeOfClientId { get; set; }
        //public DropdownDetail TypeOfClient { get; set; }
        public string TypeOfClientText { get; set; }
        public string OtherClientName { get; set; }
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
        public string ProjectStatusText { get; set; }
        public int? ProjectStatusLeadCompletedId { get; set; }
        //public DropdownDetail ProjectStatusLeadCompleted { get; set; }
        public string ProjectStatusLeadCompletedText { get; set; }
        public string ProjectStatusTotalLossRemarks { get; set; }
        public decimal ProjectStatusPartialBusinessPercentage { get; set; }
        public bool HasSwappingCompetition { get; set; }
        public int? SwappingCompetitionId { get; set; }
        //public DropdownDetail SwappingCompetition { get; set; }
        public string SwappingCompetitionText { get; set; }
        public string SwappingCompetitionAnotherCompetitorName { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftInteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftInteriorChangeReason { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int TotalPaintingAreaSqftExteriorChangeCount { get; set; }
        public string TotalPaintingAreaSqftExteriorChangeReason { get; set; }
        //public string UpTradingFromBrandName { get; set; }
        //public string UpTradingToBrandName { get; set; }
        //public string BrandUsedInteriorBrandName { get; set; }
        //public string BrandUsedExteriorBrandName { get; set; }
        //public string BrandUsedUnderCoatBrandName { get; set; }
        //public string BrandUsedTopCoatBrandName { get; set; }
        public IList<string> UpTradingFromBrandName { get; set; }
        public IList<string> UpTradingToBrandName { get; set; }
        public IList<string> BrandUsedInteriorBrandName { get; set; }
        public IList<string> BrandUsedExteriorBrandName { get; set; }
        public IList<string> BrandUsedUnderCoatBrandName { get; set; }
        public IList<string> BrandUsedTopCoatBrandName { get; set; }
        public decimal ActualPaintJobCompletedInteriorPercentage { get; set; }
        public decimal ActualPaintJobCompletedExteriorPercentage { get; set; }
        public decimal ActualVolumeSoldInteriorGallon { get; set; }
        public decimal ActualVolumeSoldInteriorKg { get; set; }
        public decimal ActualVolumeSoldExteriorGallon { get; set; }
        public decimal ActualVolumeSoldExteriorKg { get; set; }
        public decimal ActualVolumeSoldUnderCoatGallon { get; set; }
        public decimal ActualVolumeSoldTopCoatGallon { get; set; }
        //public int BusinessAchievementId { get; set; }
        public SaveLeadBusinessAchievementModel BusinessAchievement { get; set; }

        public AppSaveLeadFollowUpModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void StringToList(LeadFollowUp src, AppSaveLeadFollowUpModel dest)
        {
            dest.UpTradingFromBrandName = string.IsNullOrEmpty(src.UpTradingFromBrandName) ? new List<string>() :
                                                src.UpTradingFromBrandName.Split(',').ToList();
            dest.UpTradingToBrandName = string.IsNullOrEmpty(src.UpTradingToBrandName) ? new List<string>() :
                                                src.UpTradingToBrandName.Split(',').ToList();
            dest.BrandUsedInteriorBrandName = string.IsNullOrEmpty(src.BrandUsedInteriorBrandName) ? new List<string>() :
                                                src.BrandUsedInteriorBrandName.Split(',').ToList();
            dest.BrandUsedExteriorBrandName = string.IsNullOrEmpty(src.BrandUsedExteriorBrandName) ? new List<string>() :
                                                src.BrandUsedExteriorBrandName.Split(',').ToList();
            dest.BrandUsedTopCoatBrandName = string.IsNullOrEmpty(src.BrandUsedTopCoatBrandName) ? new List<string>() :
                                                src.BrandUsedTopCoatBrandName.Split(',').ToList();
            dest.BrandUsedUnderCoatBrandName = string.IsNullOrEmpty(src.BrandUsedUnderCoatBrandName) ? new List<string>() :
                                                src.BrandUsedUnderCoatBrandName.Split(',').ToList();
        }

        public void ListToString(AppSaveLeadFollowUpModel src, LeadFollowUp dest)
        {
            dest.UpTradingFromBrandName = src.UpTradingFromBrandName == null || !src.UpTradingFromBrandName.Any() ? string.Empty :
                                                string.Join(',', src.UpTradingFromBrandName);
            dest.UpTradingToBrandName = src.UpTradingToBrandName == null || !src.UpTradingToBrandName.Any() ? string.Empty :
                                                string.Join(',', src.UpTradingToBrandName);
            dest.BrandUsedInteriorBrandName = src.BrandUsedInteriorBrandName == null || !src.BrandUsedInteriorBrandName.Any() ? string.Empty :
                                                string.Join(',', src.BrandUsedInteriorBrandName);
            dest.BrandUsedExteriorBrandName = src.BrandUsedExteriorBrandName == null || !src.BrandUsedExteriorBrandName.Any() ? string.Empty :
                                                string.Join(',', src.BrandUsedExteriorBrandName);
            dest.BrandUsedTopCoatBrandName = src.BrandUsedTopCoatBrandName == null || !src.BrandUsedTopCoatBrandName.Any() ? string.Empty :
                                                string.Join(',', src.BrandUsedTopCoatBrandName);
            dest.BrandUsedUnderCoatBrandName = src.BrandUsedUnderCoatBrandName == null || !src.BrandUsedUnderCoatBrandName.Any() ? string.Empty :
                                                string.Join(',', src.BrandUsedUnderCoatBrandName);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, AppSaveLeadFollowUpModel>()
                .AddTransform<string>(s => string.IsNullOrEmpty(s) ? string.Empty : s)
                .ForMember(dest => dest.TypeOfClientText,
                    opt => opt.MapFrom(src => src.TypeOfClient != null ? $"{src.TypeOfClient.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusText,
                    opt => opt.MapFrom(src => src.ProjectStatus != null ? $"{src.ProjectStatus.DropdownName}" : string.Empty))
                .ForMember(dest => dest.ProjectStatusLeadCompletedText,
                    opt => opt.MapFrom(src => src.ProjectStatusLeadCompleted != null ? $"{src.ProjectStatusLeadCompleted.DropdownName}" : string.Empty))
                .ForMember(dest => dest.SwappingCompetitionText,
                    opt => opt.MapFrom(src => src.SwappingCompetition != null ? $"{src.SwappingCompetition.DropdownName}" : string.Empty))
                .ForMember(dest => dest.LastVisitedDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.LastVisitedDate)))
                .ForMember(dest => dest.NextVisitDatePlan,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.NextVisitDatePlan)))
                .ForMember(dest => dest.ActualVisitDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.ActualVisitDate)))
                .AfterMap((src, dest) => dest.StringToList(src, dest));

            profile.CreateMap<AppSaveLeadFollowUpModel, LeadFollowUp>()
                .ForMember(dest => dest.LastVisitedDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateTime(src.LastVisitedDate)))
                .ForMember(dest => dest.NextVisitDatePlan,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateTime(src.NextVisitDatePlan)))
                .ForMember(dest => dest.ActualVisitDate,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateTime(src.ActualVisitDate)))
                .AfterMap((src, dest) => src.ListToString(src, dest));

            //profile.CreateMap<LeadBusinessAchievement, SaveLeadBusinessAchievementModel>();
            //profile.CreateMap<SaveLeadBusinessAchievementModel, LeadBusinessAchievement>();
        }
    }
}
