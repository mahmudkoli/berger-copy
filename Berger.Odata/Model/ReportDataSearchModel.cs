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

    public class BrandOrDivisionWisePerformanceSearchModel
    {
        //public string CustomerNo { get; set; }
        public string Division { get; set; } // "-1" for all
        public EnumBrandOrDivision BrandOrDivision { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        //public EnumPeriod Period { get; set; }
    }

    public class DealerPerformanceSearchModel
    {
        public string Territory { get; set; }
        public EnumDealerPerformanceCategory DealerPerformanceCategory { get; set; }
        public EnumCustomerClassification DealerCategory { get; set; }
    }
    
    public class DealerPerformanceResultSearchModel
    {
        public string Territory { get; set; }
        public DealerPerformanceReportType ReportType { get; set; }
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
