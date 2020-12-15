using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DemandGeneration
{
    public class LeadGeneration : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public string Code { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int TypeOfClientId { get; set; }
        public DropdownDetail TypeOfClient { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        public int PaintingStageId { get; set; }
        public DropdownDetail PaintingStage { get; set; }
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

        public IList<LeadFollowUp> LeadFollowUps { get; set; }
    }
}
