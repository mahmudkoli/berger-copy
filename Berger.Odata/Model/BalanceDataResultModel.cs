using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;
using Newtonsoft.Json;

namespace Berger.Odata.Model
{
    public class CollectionHistoryResultModel
    {
        public string MrNo { get; internal set; }
      //  public string CustomerNo { get; internal set; }
       // public string CustomerName { get; internal set; }
        public string Division { get; internal set; }
       // public string CreditControlAreaName { get; internal set; }
        public string BankName { get; internal set; }
        public string Date { get; internal set; }
        public decimal MrAmount { get; internal set; }
        public string ChequeNo { get; internal set; }

        public CollectionHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class BalanceConfirmationSummaryResultModel
    {
        [JsonIgnore]
        public DateTime DateTime { get; set; }
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


    public class ChecqueBounceResultModel
    {
        public List<ChequeBounceSummaryResultModel> ChequeBounceSummaryResultModels { get; set; } 
        public List<ChequeBounceDetailResultModel> ChequeBounceDetailResultModels { get; set; }

        public ChecqueBounceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.ChequeBounceSummaryResultModels = new List<ChequeBounceSummaryResultModel>();
            this.ChequeBounceDetailResultModels = new List<ChequeBounceDetailResultModel>();
        }



    }


    public class ChequeBounceSummaryResultModel
    {
        public string Category { get; set; }
        public decimal MTDNoOfCheque { get; set; }
        public decimal MTDChequeValue { get; set; }
        public decimal YTDNoOfCheque { get; set; }
        public decimal YTDChequeValue { get; set; }

        public ChequeBounceSummaryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }


    public class ChequeBounceDetailResultModel
    {
      //  public string CustomerNo { get; set; }
       // public string CustomerName { get; set; }
        public string MrNumber { get; set; }
        public string ChequeNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string CreditControlArea { get; set; }
        public string CreditControlAreaName { get; internal set; }
        public string Reason { get; set; }
        //public string Remarks { get; set; }

        public ChequeBounceDetailResultModel()
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

    public class ChequeSummaryReportResultModel
    {
        //public string CustomerNo { get; set; }
        //public string CustomerName { get; set; }
        public IList<ChequeSummaryChequeDetailsReportModel> ChequeDetails { get; set; }
        public IList<ChequeSummaryChequeBounceDetailsReportModel> ChequeBounceDetails { get; set; }

        public ChequeSummaryReportResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.ChequeDetails = new List<ChequeSummaryChequeDetailsReportModel>();
            this.ChequeBounceDetails = new List<ChequeSummaryChequeBounceDetailsReportModel>();
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
        public decimal Amount { get; set; }

        public ChequeSummaryChequeBounceDetailsModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ChequeSummaryChequeDetailsReportModel
    {
        public string ChequeDetailsName { get; set; }
        public decimal MTDNoOfCheque { get; set; }
        public decimal MTDTotalChequeValue { get; set; }
        public decimal YTDNoOfCheque { get; set; }
        public decimal YTDTotalChequeValue { get; set; }

        public ChequeSummaryChequeDetailsReportModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ChequeSummaryChequeBounceDetailsReportModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Date { get; set; }
        public string ChequeNo { get; set; }
        public decimal Amount { get; set; }

        public ChequeSummaryChequeBounceDetailsReportModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
