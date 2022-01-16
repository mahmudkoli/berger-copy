using System;

namespace Berger.Odata.Model
{
    public class InvoiceHistorySearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class InvoiceDetailsSearchModel
    {
        public string InvoiceNo { get; set; }
    }

    public class BrandWiseMTDSearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public bool IsOnlyCBMaterial { get; set; }
    }

    public class BrandWisePerformanceSearchModel
    {
        public string CustomerNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Division { get; set; }
        public EnumBrandOrDivision BrandOrDivision { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
    }
}
