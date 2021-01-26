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
}
