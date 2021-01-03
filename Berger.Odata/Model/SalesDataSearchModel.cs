using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class SalesDataSearchModel
    {
        public string InvoiceNo { get; set; }
        public int Division { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ViewType { get; set; }
        public int CustomerNo { get; set; }
    }
}
