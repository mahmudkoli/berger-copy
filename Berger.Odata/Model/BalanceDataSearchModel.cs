using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class BalanceConfirmationSummarySearchModel
    {
        public string CustomerNo { get; set; }
        public string CreditControlArea { get; set; }
        public string PostingDate { get; set; }
        public string FiscalYear { get; set; }
    }

    public class ChequeBounceSearchModel
    {
        public string CustomerNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class ChequeSummarySearchModel
    {
        public string CustomerNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
