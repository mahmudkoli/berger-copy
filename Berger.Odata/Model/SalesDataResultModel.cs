using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class InvoiceHistoryResultModel
    {
        public string DealerCode { get; internal set; }
        public string DealerName { get; internal set; }
        public string CustomerNoOrSoldToParty { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string Division { get; internal set; }
        public string DivisionName { get; internal set; }
        public string InvoiceNoOrBillNo { get; internal set; }
        public string Date { get; internal set; }
        public string NetAmount { get; internal set; }
    }

    public class InvoiceItemDetailsResultModel
    {
        public string NetAmount { get; internal set; }
        public string Quantity { get; internal set; }
        public string MatrialCode { get; internal set; }
        public string MatarialDescription { get; internal set; }
    }

    public class BrandWiseMTDResultModel
    {
        public string BrandName { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal Growth { get; set; }
        public IDictionary<string, decimal> PreviousMonthData { get; set; }
    }
}
