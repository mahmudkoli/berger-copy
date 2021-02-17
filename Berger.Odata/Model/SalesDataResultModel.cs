using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class InvoiceHistoryResultModel
    {
        public string DriverName { get; internal set; }
        public string DriverMobileNo { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string Division { get; internal set; }
        public string DivisionName { get; internal set; }
        public string InvoiceNoOrBillNo { get; internal set; }
        public string Date { get; internal set; }
        public string NetAmount { get; internal set; }

        public InvoiceHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class InvoiceItemDetailsResultModel
    {
        public string NetAmount { get; internal set; }
        public string Quantity { get; internal set; }
        public string MatrialCode { get; internal set; }
        public string MatarialDescription { get; internal set; }

        public InvoiceItemDetailsResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class BrandWiseMTDResultModel
    {
        public string MatarialGroupOrBrand { get; internal set; }
        public decimal LYMTD { get; internal set; }
        public decimal CYMTD { get; internal set; }
        public decimal Growth { get; internal set; }
        public IDictionary<string, decimal> PreviousMonthData { get; internal set; }

        public BrandWiseMTDResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class BrandOrDivisionWiseMTDResultModel
    {
        public string MatarialGroupOrBrandOrDivision { get; internal set; }
        public decimal LYSM { get; internal set; }
        public decimal LYMTD { get; internal set; }
        public decimal LYYTD { get; internal set; }
        public decimal CYMTD { get; internal set; }
        public decimal CYYTD { get; internal set; }
        public decimal GrowthMTD { get; internal set; }
        public decimal GrowthYTD { get; internal set; }

        public BrandOrDivisionWiseMTDResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
