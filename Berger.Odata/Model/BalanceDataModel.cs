using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class BalanceDataModel
    {
        public string CompanyCode { get; set; }
        public string CustomerLow { get; set; }
        public string CustomerHigh { get; set; }
        public string PostingDate { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string PostingDateDoc { get; set; }
        public string FiscalYear { get; set; }
        public string CreditControlArea { get; set; }
        public string AccDocNo { get; set; }
        public string DocType { get; set; }
        public string Dzblart { get; set; }
        public string Amount { get; set; }
        public string TransactionDescription { get; set; }
        public string LineText { get; set; }
        public string ChequeBounceStatus { get; set; }
        public string BankNo { get; set; }
        public string ChequeNo { get; set; }
    }
}
