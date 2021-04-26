using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CollectionHistoryResultModel
    {
        public string DocumentNo { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string CreditControlArea { get; internal set; }
        public string CreditControlAreaName { get; internal set; }
        public string BankName { get; internal set; }
        public string PostingDate { get; internal set; }
        public decimal Amount { get; internal set; }
        public string InstrumentNo { get; internal set; }

        public CollectionHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class BalanceConfirmationSummaryResultModel
    {
        public string Date { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal InvoiceBalance { get; set; }
        public decimal PaymentBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string TransactionDescription { get; set; }

        public BalanceConfirmationSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ChequeBounceResultModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string DocumentNo { get; set; }
        public string InstrumentNo { get; set; }
        public string ReversalDate { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string CreditControlArea { get; set; }
        public string CreditControlAreaName { get; internal set; }
        public string Reason { get; set; }
        //public string Remarks { get; set; }

        public ChequeBounceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ChequeSummaryResultModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public IList<ChequeSummaryChequeDetailsModel> ChequeDetails { get; set; }
        public IList<ChequeSummaryChequeBounceDetailsModel> ChequeBounceDetails { get; set; }

        public ChequeSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.ChequeDetails = new List<ChequeSummaryChequeDetailsModel>();
            this.ChequeBounceDetails = new List<ChequeSummaryChequeBounceDetailsModel>();
        }
    }

    public class ChequeSummaryChequeDetailsModel
    {
        public string ChequeDetailsName { get; set; }
        public decimal MTDNoOfCheque { get; set; }
        public decimal MTDTotalChequeValue { get; set; }
        public decimal YTDNoOfCheque { get; set; }
        public decimal YTDTotalChequeValue { get; set; }

        public ChequeSummaryChequeDetailsModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ChequeSummaryChequeBounceDetailsModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string ReversalDate { get; set; }
        public string ChequeNo { get; set; }
        public string Amount { get; set; }

        public ChequeSummaryChequeBounceDetailsModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
