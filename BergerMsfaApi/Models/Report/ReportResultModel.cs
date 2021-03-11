using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Report
{
    public class LeadSummaryReportResultModel
    {
        public string UserId { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int NoOfLeadGenerate { get; set; }
        public int NoOfLeadFollowUp { get; set; }
        public int TotalCall { get; set; }
        public int NoOfUnderConstructionLead { get; set; }
        public int NoOfGoingPaintLead { get; set; }
        public int NoOfTotalWinLead { get; set; }
        public int NoOfTotalLossLead { get; set; }
        public int NoOfPartialBusinessLead { get; set; }
        public int NoOfCompetitionSnatchLead { get; set; }
        public decimal BergerValueSales { get; set; }
        public decimal BergerPremiumBrandValueSales { get; set; }
        public decimal CompetitionValueSales { get; set; }
        public int NoOfColorSchemeGiven { get; set; }
        public int NoOfProductSampling { get; set; }
    }

    public class LeadGenerationDetailsReportResultModel
    {
        public string UserId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Depot { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string LeadCreatedDate { get; set; }
        public string TypeOfClient { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        public string PaintingStage { get; set; }
        public string ExpectedDateOfPainting { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public decimal ExpectedValue { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        public string RequirementOfColorScheme { get; set; }
        public string ProductSamplingRequired { get; set; }
        public string NextFollowUpDate { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }
    }

    public class LeadFollowUpDetailsReportResultModel
    {
        public string UserId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Depot { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string PlanVisitDatePlan { get; set; }
        public string ActualVisitDate { get; set; }
        public string TypeOfClient { get; set; }
        public string ProjectAddress { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        public int NumberOfStoriedBuilding { get; set; }
        public decimal ExpectedValue { get; set; }
        public decimal ExpectedMonthlyBusinessValue { get; set; }
        public string SwappingCompetition { get; set; }
        public string SwappingCompetitionAnotherCompetitorName { get; set; }
        public string UpTradingFromBrandName { get; set; }
        public string UpTradingToBrandName { get; set; }
        public string BrandUsedInteriorBrandName { get; set; }
        public string BrandUsedExteriorBrandName { get; set; }
        public string BrandUsedUnderCoatBrandName { get; set; }
        public string BrandUsedTopCoatBrandName { get; set; }
        public int TotalPaintingAreaSqftInterior { get; set; }
        public int TotalPaintingAreaSqftExterior { get; set; }
        public int ActualPaintJobCompletedInterior { get; set; }
        public int ActualPaintJobCompletedExterior { get; set; }
        public int ActualVolumeSoldInteriorGallon { get; set; }
        public int ActualVolumeSoldInteriorKg { get; set; }
        public int ActualVolumeSoldExteriorGallon { get; set; }
        public int ActualVolumeSoldExteriorKg { get; set; }
        public int ActualVolumeSoldUnderCoatGallon { get; set; }
        public int ActualVolumeSoldTopCoatGallon { get; set; }
        public decimal BergerValueSales { get; set; }
        public decimal BergerPremiumBrandSalesValue { get; set; }
        public decimal CompetitionValueSales { get; set; }
        public string ProductSourcing { get; set; }
        public string ProjectStatus { get; set; }
        public string IsColorSchemeGiven { get; set; }
        public string IsProductSampling { get; set; }
        public string NextVisitDate { get; set; }
        public string Comments { get; set; }
        public string ImageUrl { get; set; }
    }

    #region Nasir
    public class PainterRegistrationReportResultModel
    {
        public string UserId { get; set; }
        public string PainterId { get; set; }
        public string PainerRegistrationDate { get; set; }
        public string TypeOfPainer { get; set; }
        public string DepotName { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string PainterName { get; set; }
        public string PainterAddress { get; set; }
        public string MobileNumber { get; set; }
        public int NoOfPaintingAttached { get; set; }
        public string DBBLRocketAccountStatus { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string IdentificationNo { get; set; }
        public string AttachedTaggedDealerId { get; set; }
        public string AttachedTaggedDealerName { get; set; }
        public string APPInstalledStatus { get; set; }
        public string APPNotInstalledReason { get; set; }
        public string AverageMonthlyUse { get; set; }
        public string BergerLoyalty { get; set; }
        public string PainterImageUrl { get; set; }
    }

    public class DealerOpeningReportResultModel
    {
        public string UserId { get; set; }
        public string DealrerOpeningId { get; set; }
        public string BusinessArea { get; set; }
        public string BusinessAreaName { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string EmployeeId { get; set; }
        public string DealershipOpeningApplicationForm { get; set; }
        public string TradeLicensee { get; set; }
        public string IdentificationNo { get; set; }
        public string PhotographOfproprietor { get; set; }
        public string NomineeIdentificationNo { get; set; }
        public string NomineePhotograph { get; set; }
        public string Cheque { get; set; }
        public string CurrentStatusOfThisApplication { get; set; }
    }

    public class DealerCollectionReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CollectionDate { get; set; }
        public string TypeOfCustomer { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditControlArea { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal CashAmount { get; set; }
        public string ManualMrNumber { get; set; }
        public string Remarks { get; set; }
    }

    public class SubDealerCollectionReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CollectionDate { get; set; }
        public string TypeOfCustomer { get; set; }
        public string SubDealerCode { get; set; }
        public string SubDealerName { get; set; }
        public string SubDealerMobileNumber { get; set; }
        public string SubDealerAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditControlArea { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal CashAmount { get; set; }
        public string ManualMrNumber { get; set; }
        public string Remarks { get; set; }
    }

    public class CustomerCollectionReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CollectionDate { get; set; }
        public string TypeOfCustomer { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditControlArea { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal CashAmount { get; set; }
        public string ManualMrNumber { get; set; }
        public string Remarks { get; set; }
    }

    public class DirectProjectCollectionReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CollectionDate { get; set; }
        public string TypeOfCustomer { get; set; }
        public string ProjectSapId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditControlArea { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal CashAmount { get; set; }
        public string ManualMrNumber { get; set; }
        public string Remarks { get; set; }
    }

    #endregion
}
