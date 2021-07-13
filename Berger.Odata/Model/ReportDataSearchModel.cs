using System.Collections.Generic;

namespace Berger.Odata.Model
{
    public class AppAreaSearchCommonModel
    {
        public IList<string> Depots { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }

        public AppAreaSearchCommonModel()
        {
            this.Depots = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }

    public class TodaysInvoiceValueSearchModel
    {
        public string Division { get; set; }
    }

    public class MTDTargetSummarySearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public EnumBrandCategory? Category { get; set; }
        public string Division { get; set; }
    }

    public class MTDBrandPerformanceSearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public EnumBrandType Type { get; set; }
        public string Division { get; set; }
    }

    public class YTDBrandPerformanceSearchModelSearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Division { get; set; }
        public EnumBrandOrDivision BrandOrDivision { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
    }

    public class CategoryWiseDealerPerformanceSearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumDealerPerformanceCategory PerformanceCategory { get; set; }
        public EnumCustomerClassification Category { get; set; }
    }

    public class OutstandingSummaryReportSearchModel : AppAreaSearchCommonModel
    {
        public string CreditControlArea { get; set; }
    }
    
    public class DealerPerformanceResultSearchModel
    {
        public string Territory { get; set; }
        public DealerPerformanceReportType ReportType { get; set; }
    } 
    
    public class LastYearAppointedDealerPerformanceSearchModel: AppAreaSearchCommonModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public LastYearAppointedDealerPerformanceType ReportType { get; set; }
    }
    
    public class RprsFollowupSearchModel: AppAreaSearchCommonModel
    {
        public List<string> Division { get; set; } = new List<string>();
        public List<string> Dealer { get; set; } = new List<string>();
        public LastYearAppointedDealerPerformanceType ReportType { get; set; }
    }

    public class OSOver90DaysSearchModel
    {
        public string CreditControlArea { get; set; }
    }

    public class PaymentFollowUpSearchModel
    {
        public EnumPaymentFollowUpType PaymentFollowUpType { get; set; }
    }
}
