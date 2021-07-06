using Berger.Common.Extensions;

namespace Berger.Odata.Model
{
    public class TargetReportResultModel
    {
        public string TerritoryNumber { get; set; }
        public string Zone { get; set; }
        public string Brand { get; set; }
        public decimal LYSMAchieved { get; set; }
        public decimal TotalMTSTarget { get; set; }
        public decimal TillDateTarget { get; set; }
        public decimal TillDateMTSAchieved { get; set; }
        public decimal DayTarget { get; set; }
        public decimal DaySales { get; set; }
        public decimal TillDateIdealAchieved { get; set; }
        public decimal TillDateActualAchieved { get; set; }
        public string Category { get; set; }
    }

    public class MySummaryReportResultModel
    {
        public int DealerVisitTarget { get; set; }
        public int DealerVisitActual { get; set; }
        public int SubDealerVisitTarget { get; set; }
        public int SubDealerVisitActual { get; set; }
        public int AdHocDealerVisit { get; set; }
        public int AdHocSubDealerVisit { get; set; }
        public int NoOfBillingDealer { get; set; } // Division & Channel = 10
        public int PainterCall { get; set; }
        public int CollectionFromDealer { get; set; }
        public int CollectionFromSubDealer { get; set; }
        public int CollectionFromDirectProject { get; set; }
        public int CollectionFromCustomer { get; set; }
        public int LeadGenerationNo { get; set; }
        public int LeadFollowupNo { get; set; }
        public decimal DGABusinessValue { get; set; }
    } 

    public class TotalInvoiceValueResultModel
    {
        public string InvoiceNoOrBillNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class BrandOrDivisionWisePerformanceResultModel
    {
        public string MatarialGroupOrBrandOrDivision { get; internal set; }
        //public decimal LYSM { get; internal set; }
        public decimal LYMTD { get; internal set; }
        public decimal LYYTD { get; internal set; }
        public decimal CYMTD { get; internal set; }
        public decimal CYYTD { get; internal set; }
        public decimal GrowthMTD { get; internal set; }
        public decimal GrowthYTD { get; internal set; }

        public BrandOrDivisionWisePerformanceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class DealerPerformanceResultModel
    {
        public int SLNo { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public decimal LYSales { get; internal set; }
        public decimal CYSales { get; internal set; }
        public decimal Growth { get; internal set; }

        public DealerPerformanceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ReportOutstandingSummaryResultModel
    {
        public string CreditControlArea { get; internal set; }
        public string CreditControlAreaName { get; internal set; }
        public decimal ValueLimit { get; internal set; }
        public decimal NetDue { get; internal set; }
        public decimal Slippage { get; internal set; }
        public decimal OSOver90Days { get; internal set; }

        public ReportOutstandingSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ReportDealerPerformanceResultModel
    {
        public string Territory { get; set; }
        public int NumberOfDealer { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD  { get; set; }
        public decimal LYYTD  { get; set; }
        public decimal CYYTD   { get; set; }
        public decimal GrowthMTD   { get; set; }
        public decimal GrowthYTD    { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class ReportOSOver90DaysResultModel
    {
        public string FirstMonthName { get; internal set; }
        public string SecondMonthName { get; internal set; }
        public string ThirdMonthName { get; internal set; }
        public decimal FirstMonthAmount { get; internal set; }
        public decimal SecondMonthAmount { get; internal set; }
        public decimal ThirdMonthAmount { get; internal set; }
        public decimal SecondMonthChangeAmount { get; internal set; }
        public decimal ThirdMonthChangeAmount { get; internal set; }

        public ReportOSOver90DaysResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ReportPaymentFollowUpResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string InvoiceNo { get; internal set; }
        public string InvoiceDate { get; internal set; }
        public string InvoiceAge { get; internal set; }
        public string DayLimit { get; internal set; }
        public string RPRSDate { get; internal set; }

        public ReportPaymentFollowUpResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
