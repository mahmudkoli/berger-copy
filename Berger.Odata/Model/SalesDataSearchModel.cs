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

    public class BrandOrDivisionWiseMTDSearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; } // "-1" for all
        public EnumBrandOrDivision BrandOrDivision { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        //public EnumPeriod Period { get; set; }
    }
}
