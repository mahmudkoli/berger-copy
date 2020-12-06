﻿using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DemandGeneration
{
    public class LeadFollowUp : AuditableEntity<int>
    {
        public int LeadGenerationId { get; set; }
        public LeadGeneration LeadGeneration { get; set; }
        //public string Depot { get; set; }
        //public string Territory { get; set; }
        //public string Zone { get; set; }
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
        public string ExpectedValueChangeReason { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
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
        public int BusinessAchievementId { get; set; }
        public LeadBusinessAchievement BusinessAchievement { get; set; }
    }
}