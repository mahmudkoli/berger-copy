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

    public class MyTargetSearchModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public MyTargetReportType ReportType { get; set; }
        public string Division { get; set; }
        public EnumMyTargetBrandType BrandType { get; set; }
    }

    public class MTDTargetSummarySearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public EnumBrandCategoryType? Category { get; set; }
        public string Division { get; set; }
    }

    public enum EnumBrandCategoryType
    {
        Liquid = 1,
        Powder = 2
    }

    public enum MyTargetReportType
    {
        TerritoryWiseTarget = 1,
        ZoneWiseTarget = 2,
        BrandWise = 3
    }


    public class TotalInvoiceValueSearchModel
    {
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
        public EnumDealerClassificationCategory DealerCategory { get; set; }
    }

    public enum EnumDealerPerformanceCategory
    {
        Top_10_Performer = 1,
        Bottom_10_Performer = 2,
        NotPurchasedLastMonth = 3,
    }

    public enum EnumDealerClassificationCategory
    {
        All = 1,
        Exclusive = 2,
        NonExclusive = 3,
    }
    
    public class DealerPerformanceResultSearchModel
    {
        public string Territory { get; set; }
        public DealerPerformanceReportType ReportType { get; set; }
    }

    public enum DealerPerformanceReportType
    {
        LastYearAppointed = 1,
        ClubSupremeTerritoryWise = 2,
        ClubSupremeTerritoryAndDealerWise = 3,
    }

    public class OSOver90DaysSearchModel
    {
        public string CreditControlArea { get; set; }
    }

    public class PaymentFollowUpSearchModel
    {
        public EnumPaymentFollowUpTypeModel PaymentFollowUpType { get; set; }
    }

    public enum EnumPaymentFollowUpTypeModel
    {
        RPRS = 1,
        FastPayCarry = 2
    }

    public enum EnumMyTargetBrandType
    {
        All_Brands = 1,
        MTS_Brands = 2
    }
}
