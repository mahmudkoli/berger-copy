using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class InvoiceHistorySearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class InvoiceItemDetailsSearchModel
    {
        public string InvoiceNo { get; set; }
    }

    public class BrandWiseMTDSearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public DateTime Date { get; set; }
    }

    public class BrandOrDivisionWiseMTDSearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; } // "-1" for all
        public EnumBrandOrDivision BrandOrDivision { get; set; }
        public string Brand { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        //public EnumPeriod Period { get; set; }
        public DateTime Date { get; set; }
    }

    public enum EnumBrandOrDivision
    {
        All_Brand=1,
        MTS_Brand=2,
        Division=3,
    }

    public enum EnumVolumeOrValue
    {
        Volume = 1,
        Value = 2,
    }

    //public enum EnumPeriod
    //{
    //    Fiscal_Year_Apr_Mar = 1,
    //}
}
