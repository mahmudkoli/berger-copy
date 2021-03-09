using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CollectionHistoryResultModel
    {
        public string InvoiceNo { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string Division { get; internal set; }
        public string BankName { get; internal set; }
        public string PostingDate { get; internal set; }
        public string Amount { get; internal set; }
        public string InstrumentNo { get; internal set; }

        public CollectionHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class OutstandingDetailsResultModel
    {
        public string InvoiceNo { get; internal set; }
        public string Age { get; internal set; }
        public string PostingDate { get; internal set; }
        public string Amount { get; internal set; }

        public OutstandingDetailsResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
