using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class BalanceConfirmationSummaryResultModel
    {
        public string Date { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal InvoiceBalance { get; set; }
        public decimal PaymentBalance { get; set; }
        public decimal ClosingBalance { get; set; }

        public BalanceConfirmationSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
