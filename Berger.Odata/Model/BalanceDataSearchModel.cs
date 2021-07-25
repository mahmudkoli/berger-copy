using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CollectionHistorySearchModel
    {
        public string CustomerNo { get; set; }
        public string CreditControlArea { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class BalanceConfirmationSummarySearchModel
    {
        public string CustomerNo { get; set; }
        public string CreditControlArea { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
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

    public class ChequeSummaryReportSearchModel : AppAreaSearchCommonModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public IList<string> CustomerNos { get; set; }
    }
}
