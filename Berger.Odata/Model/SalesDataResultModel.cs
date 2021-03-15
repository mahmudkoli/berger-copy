using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class InvoiceHistoryResultModel
    {
        //    public string DriverName { get; internal set; }
        //    public string DriverMobileNo { get; internal set; }
        //    public string CustomerNo { get; internal set; }
        //    public string CustomerName { get; internal set; }
        //    public string Division { get; internal set; }
        //    public string DivisionName { get; internal set; }
        public string InvoiceNoOrBillNo { get; internal set; }
        public string Date { get; internal set; }
        public decimal NetAmount { get; internal set; }
        //public string Currency { get; internal set; }
        public string Time { get; internal set; }

        public InvoiceHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class InvoiceDetailsResultModel
    {
        public string InvoiceNoOrBillNo { get; internal set; }
        public string Date { get; internal set; }
        public decimal NetAmount { get; internal set; }
        public string DriverName { get; internal set; }
        public string DriverMobileNo { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string Division { get; internal set; }
        public string DivisionName { get; internal set; }
        public IList<InvoiceItemDetailsResultModel> InvoiceItemDetails { get; set; }

        public InvoiceDetailsResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.InvoiceItemDetails = new List<InvoiceItemDetailsResultModel>();
        }
    }

    public class InvoiceItemDetailsResultModel
    {
        public decimal NetAmount { get; internal set; }
        public decimal Quantity { get; internal set; }
        public string MatrialCode { get; internal set; }
        public string MatarialDescription { get; internal set; }
        public string Unit { get; internal set; }
        public string LineNumber { get; internal set; }
        //public string Currency { get; internal set; }

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
        public IList<BrandWiseMTDPreviousModel> PreviousMonthData { get; internal set; }

        public BrandWiseMTDResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.PreviousMonthData = new List<BrandWiseMTDPreviousModel>();
        }
    }

    public class BrandWiseMTDPreviousModel
    {
        public string MonthName { get; internal set; }
        public decimal Amount { get; internal set; }

        public BrandWiseMTDPreviousModel()
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
