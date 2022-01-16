using Berger.Common.Extensions;
using System;
using System.Collections.Generic;
using NJ = Newtonsoft.Json;
using SJ = System.Text.Json.Serialization;

namespace Berger.Odata.Model
{
    public class TodaysActivitySummaryReportResultModel
    {
        public int DealerVisitTarget { get; set; }
        public int DealerVisitActual { get; set; }
        public int SubDealerVisitTarget { get; set; }
        public int SubDealerVisitActual { get; set; }
        public int AdHocDealerVisit { get; set; }
        public int AdHocSubDealerVisit { get; set; }
        public int NoOfBillingDealer { get; set; }
        public int PainterCall { get; set; }
        public int CollectionFromDealer { get; set; }
        public int CollectionFromSubDealer { get; set; }
        public int CollectionFromDirectProject { get; set; }
        public int CollectionFromCustomer { get; set; }
        public int LeadGenerationNo { get; set; }
        public int LeadFollowupNo { get; set; }
        public decimal DGABusinessValue { get; set; }
    }

    public class TodaysInvoiceValueResultModel
    {
        public string InvoiceNoOrBillNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class MTDTargetSummaryReportResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public IList<string> Depots { get; set; }
        public string Depot { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CMTarget { get; set; }
        public decimal CMActual { get; set; }
        public decimal TillDateGrowth { get; set; }
        public decimal AskingPerDay { get; set; }
        public decimal TillDatePerformacePerDay { get; set; }

        public MTDTargetSummaryReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class MTDBrandPerformanceReportResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public IList<string> Depots { get; set; }
        public string Depot { get; set; }
        public string Brand { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CMTarget { get; set; }
        public decimal CMActual { get; set; }
        public decimal TillDateGrowth { get; set; }
        public decimal AskingPerDay { get; set; }
        public decimal TillDatePerformacePerDay { get; set; }

        public MTDBrandPerformanceReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class YTDBrandPerformanceSearchModelResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public IList<string> Depots { get; set; }
        public string Depot { get; set; }
        public string BrandOrDivision { get; internal set; }
        public decimal LYMTD { get; internal set; }
        public decimal LYYTD { get; internal set; }
        public decimal CYMTD { get; internal set; }
        public decimal CYYTD { get; internal set; }
        public decimal GrowthMTD { get; internal set; }
        public decimal GrowthYTD { get; internal set; }

        public YTDBrandPerformanceSearchModelResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class CategoryWiseDealerPerformanceResultModel
    {
        public int Ranking { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public decimal LYMTD { get; internal set; }
        public decimal LYYTD { get; internal set; }
        public decimal CYMTD { get; internal set; }
        public decimal CYYTD { get; internal set; }
        public decimal GrowthMTD { get; internal set; }
        public decimal GrowthYTD { get; internal set; }

        public CategoryWiseDealerPerformanceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class OutstandingSummaryReportResultModel
    {
        public string CreditControlArea { get; internal set; }
        public decimal ValueLimit { get; internal set; }
        public decimal NetDue { get; internal set; }
        public decimal Slippage { get; internal set; }
        public decimal OSOver90Days { get; internal set; }

        public OutstandingSummaryReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ReportDealerPerformanceResultModel
    {
        public string Territory { get; set; }
        public int NumberOfDealer { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
        public decimal GrowthMTD { get; set; }
        public decimal GrowthYTD { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class RptLastYearAppointDlerPerformanceSummaryResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public string DepotCode { get; set; }
        public string Depot { get; set; }
        public int NumberOfDealer { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal GrowthMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
        public decimal GrowthYTD { get; set; }
    }


    public class ReportClubSupremePerformance
    {
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal GrowthMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
        public decimal GrowthYTD { get; set; }
        public string ClubStatus { get; set; }

    }


    public class ReportClubSupremePerformanceSummary : ReportClubSupremePerformance
    {
        public int NumberOfDealer { get; set; }
    }

    public class ReportClubSupremePerformanceDetail : ReportClubSupremePerformance
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public string DepotCode { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }


    public class RptLastYearAppointDlrPerformanceDetailResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public string DepotCode { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal GrowthMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
        public decimal GrowthYTD { get; set; }
    }

    public class OSOver90DaysTrendReportResultModel
    {
        public string Month { get; internal set; }
        public decimal OSOver90Days { get; internal set; }
        public decimal Difference { get; internal set; }
        public decimal Sales { get; internal set; }
        public decimal OSPercentageWithSales { get; internal set; }

        public OSOver90DaysTrendReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class PaymentFollowUpResultModel
    {
        [SJ.JsonIgnore]
        [NJ.JsonIgnore]
        public DateTime InvoiceDateTime { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string InvoiceDate { get; internal set; }
        public string InvoiceNo { get; internal set; }
        public decimal NetDue { get; set; }
        public int InvoiceAge { get; internal set; }
        public int DayLimit { get; internal set; }
        public int DayLimitRPRS { get; internal set; }
        public string RPRSDate { get; internal set; }

        public PaymentFollowUpResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
