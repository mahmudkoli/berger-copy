namespace Berger.Odata.Model
{
    public class MyTargetSearchModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public MyTargetReportType ReportType { get; set; }
        public string Division { get; set; }
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
}
