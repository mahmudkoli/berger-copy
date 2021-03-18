using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class OutstandingDetailsResultModel
    {
        public string InvoiceNo { get; internal set; }
        public string Age { get; internal set; }
        public string PostingDate { get; internal set; }
        public decimal Amount { get; internal set; }

        public OutstandingDetailsResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class OutstandingSummaryResultModel
    {
        public string CreditControlArea { get; internal set; }
        public string CreditControlAreaName { get; internal set; }
        public string DaysLimit { get; internal set; }
        public decimal ValueLimit { get; internal set; }
        public decimal NetDue { get; internal set; }
        public decimal Slippage { get; internal set; }
        public string HighestDaysInvoice { get; internal set; }

        public OutstandingSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
