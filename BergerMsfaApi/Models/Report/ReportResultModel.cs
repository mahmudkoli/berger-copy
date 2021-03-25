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
        public decimal ActualPaintJobCompletedInterior { get; set; }
        public decimal ActualPaintJobCompletedExterior { get; set; }
        public decimal ActualVolumeSoldInteriorGallon { get; set; }
        public decimal ActualVolumeSoldInteriorKg { get; set; }
        public decimal ActualVolumeSoldExteriorGallon { get; set; }
        public decimal ActualVolumeSoldExteriorKg { get; set; }
        public decimal ActualVolumeSoldUnderCoatGallon { get; set; }
        public decimal ActualVolumeSoldTopCoatGallon { get; set; }
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

    public class PainterCallReportResultModel
    {
        public string UserId { get; set; }
        public string PainterId { get; set; }
        public string PainterVisitDate { get; set; }
        public string TypeOfPainter { get; set; }
        public string DepotName { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string PainterName { get; set; }
        public string PainterAddress { get; set; }
        public string MobileNumber { get; set; }
        public string NoOfPainterAttached { get; set; }
        public string DbblRocketAccountStatus { get; set; }
        public string AccountNumber { get; set; }
        public string AcccountHolderName { get; set; }
        public string IdentificationNo { get; set; }
        public string AttachedTaggedDealerId { get; set; }
        public string AttachedTaggedDealerName { get; set; }
        public string ShamparkaAppInstallStatus { get; set; }
        public string BergerLoyalty { get; set; }
        public string PainterSchemeCommunication { get; set; }
        public string PremiumProductBriefing { get; set; }
        public string NewProductBriefing { get; set; }
        public string EpToolsUsage { get; set; }
        public string PainterAppUsage { get; set; }
        public string WorkInHandNo { get; set; }
        public string ApMtdValue { get; set; }
        public string ApCount { get; set; }
        public string NerolacMtdValue { get; set; }
        public string NerolacCount { get; set; }
        public string EliteMtdValue { get; set; }
        public string EliteCount { get; set; }
        public string NipponMtdValue { get; set; }
        public string NipponCount { get; set; }
        public string DuluxMtdValue { get; set; }
        public string DuluxCount { get; set; }
        public string MoonstarMtdValue { get; set; }
        public string MoonstarCount { get; set; }
        public string OthersMtdValue { get; set; }
        public string OthersCount { get; set; }
        public string TotalMtdValue { get; set; }
        public string TotalCount { get; set; }
        public string IssueWithDbblAccount { get; set; }
        public string RemarkIssueWithDbblAccount { get; set; }
        public string Comments { get; set; }
    }

    public class DealerVisitReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D5 { get; set; }
        public string D6 { get; set; }
        public string D7 { get; set; }
        public string D8 { get; set; }
        public string D9 { get; set; }
        public string D10 { get; set; }
        public string D11 { get; set; }
        public string D12 { get; set; }
        public string D13 { get; set; }
        public string D14 { get; set; }
        public string D15 { get; set; }
        public string D16 { get; set; }
        public string D17 { get; set; }
        public string D18 { get; set; }
        public string D19 { get; set; }
        public string D20 { get; set; }
        public string D21 { get; set; }
        public string D22 { get; set; }
        public string D23 { get; set; }
        public string D24 { get; set; }
        public string D25 { get; set; }
        public string D26 { get; set; }
        public string D27 { get; set; }
        public string D28 { get; set; }
        public string D29 { get; set; }
        public string D30 { get; set; }
        public string D31 { get; set; }
        public int TargetVisits { get; set; }
        public int ActualVisits { get; set; }
        public int NotVisits { get; set; }
    }

    public class DealerSalesCallReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string VisitDate { get; set; }
        public string TradePromotion { get; set; }
        public string Target { get; set; }
        public string SsStatus { get; set; }
        public string SsReasonForPourOrAverage { get; set; }
        public string OsCommunication { get; set; }
        public string SlippageCommunication { get; set; }
        public string UspCommunication { get; set; }
        public string ProductLiftingStatus { get; set; }
        public string ReasonForNotLifting { get; set; }
        public string CbMachineStatus { get; set; }
        public string CbProductivity { get; set; }
        public string Merchendising { get; set; }
        public string SubDealerInfluence { get; set; }
        public string SdInfluecePercent { get; set; }
        public string PainterInfluence { get; set; }
        public string PainterInfluecePercent { get; set; }
        public string ProductKnoledge { get; set; }
        public string SalesTechniques { get; set; }
        public string MerchendisingImprovement { get; set; }
        public string CompetitionPresence { get; set; }
        public string CompetitionService { get; set; }
        public string CsRemarks { get; set; }
        public string ProductDisplayAndMerchendizingStatus { get; set; }
        public string PdmRemarks { get; set; }
        public string ProductDisplayAndMerchendizingImage { get; set; }
        public string SchemeModality { get; set; }
        public string SchemeModalityImage { get; set; }
        public string ShopBoy { get; set; }
        public string ApAvrgMonthlySales { get; set; }
        public string ApActualMtdSales { get; set; }
        public string NerolacAvrgMonthlySales { get; set; }
        public string NerolacActualMtdSales { get; set; }
        public string NipponAvrgMonthlySales { get; set; }
        public string NipponActualMtdSales { get; set; }
        public string DuluxAvrgMonthlySales { get; set; }
        public string DuluxActualMtdSales { get; set; }
        public string JotunAvrgMonthlySales { get; set; }
        public string JotunActualMtdSales { get; set; }
        public string MoonstarAvrgMonthlySales { get; set; }
        public string MoonstarActualMtdSales { get; set; }
        public string EliteAvrgMonthlySales { get; set; }
        public string EliteActualMtdSales { get; set; }
        public string AlkarimAvrgMonthlySales { get; set; }
        public string AlkarimActualMtdSales { get; set; }
        public string OthersAvrgMonthlySales { get; set; }
        public string OthersActualMtdSales { get; set; }
        public string TotalAvrgMonthlySales { get; set; }
        public string TotalActualMtdSales { get; set; }
        public string DealerIssueStatus { get; set; }
        public string DealerSatisfactionStatus { get; set; }
        public string DealerDissatisfactionReason { get; set; }
    }

    public class SubDealerSalesCallReportResultModel
    {
        public string UserId { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string SubDealerId { get; set; }
        public string SubDealerName { get; set; }
        public string VisitDate { get; set; }
        public string TradePromotion { get; set; }
        public string SsStatus { get; set; }
        public string SsReasonForPourOrAverage { get; set; }
        public string OsStatus { get; set; }
        public string OsActivity { get; set; }
        public string UspCommunication { get; set; }
        public string ProductLiftingStatus { get; set; }
        public string ReasonForNotLifting { get; set; }
        public string Merchendising { get; set; }
        public string PainterInfluence { get; set; }
        public string PainterInfluecePercent { get; set; }
        public string ProductKnoledge { get; set; }
        public string SalesTechniques { get; set; }
        public string MerchendisingImprovement { get; set; }
        public string BergerAvrgMonthlySales { get; set; }
        public string BergerActualMtdSales { get; set; }
        public string CompetitionPresence { get; set; }
        public string CompetitionService { get; set; }
        public string CsRemarks { get; set; }
        public string ProductDisplayAndMerchendizingStatus { get; set; }
        public string PdmRemarks { get; set; }
        public string ProductDisplayAndMerchendizingImage { get; set; }
        public string SchemeModality { get; set; }
        public string SchemeModalityImage { get; set; }
        public string ShopBoy { get; set; }
        public string ApAvrgMonthlySales { get; set; }
        public string ApActualMtdSales { get; set; }
        public string NerolacAvrgMonthlySales { get; set; }
        public string NerolacActualMtdSales { get; set; }
        public string NipponAvrgMonthlySales { get; set; }
        public string NipponActualMtdSales { get; set; }
        public string DuluxAvrgMonthlySales { get; set; }
        public string DuluxActualMtdSales { get; set; }
        public string JotunAvrgMonthlySales { get; set; }
        public string JotunActualMtdSales { get; set; }
        public string MoonstarAvrgMonthlySales { get; set; }
        public string MoonstarActualMtdSales { get; set; }
        public string EliteAvrgMonthlySales { get; set; }
        public string EliteActualMtdSales { get; set; }
        public string AlkarimAvrgMonthlySales { get; set; }
        public string AlkarimActualMtdSales { get; set; }
        public string OthersAvrgMonthlySales { get; set; }
        public string OthersActualMtdSales { get; set; }
        public string TotalAvrgMonthlySales { get; set; }
        public string TotalActualMtdSales { get; set; }
        public string SubDealerIssueStatus { get; set; }
        public string DealerSatisfactionStatus { get; set; }
        public string DealerDissatisfactionReason { get; set; }
    }

    #endregion

    public class OsOver90daysTrendReportResultModel
    {

        public string CreditControlArea { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string Month1Name { get; set; }
        public string Month2Name { get; set; }
        public string Month3Name { get; set; }
        public decimal Month1Value { get; set; }
        public decimal Month2Value { get; set; }
        public decimal Change1 { get; set; }
        public decimal Month3Value { get; set; }
        public decimal Change2 { get; set; }
    }


}
