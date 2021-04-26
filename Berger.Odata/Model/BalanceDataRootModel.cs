using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class BalanceDataRootModel
    {
        public string CompanyCode { get; set; }
        public string CustomerLow { get; set; }
        public string CustomerHigh { get; set; }
        public string PostingDate { get; set; }
        public string Kunnr { get; set; }
        public string Name1 { get; set; }
        public string Budat { get; set; }
        public string Gjahr { get; set; }
        public string Kkber { get; set; }
        public string Belnr { get; set; }
        public string Blart { get; set; }
        public string Dzblart { get; set; }
        public string Dmbtr { get; set; }
        public string Sgtxt { get; set; }
        public string Linetext { get; set; }
        public string Bouncestatus { get; set; }
        public string Bank { get; set; }
        public string Chequeno { get; set; }

        public BalanceDataModel ToModel()
        {
            var model = new BalanceDataModel();
            model.CompanyCode = this.CompanyCode;
            model.CustomerLow = this.CustomerLow;
            model.CustomerHigh = this.CustomerHigh;
            model.PostingDate = this.PostingDate;
            model.CustomerNo = this.Kunnr;
            model.CustomerName = this.Name1;
            model.PostingDateDoc = this.Budat;
            model.FiscalYear = this.Gjahr;
            model.CreditControlArea = this.Kkber;
            model.AccDocNo = this.Belnr;
            model.DocType = this.Blart;
            model.Dzblart = this.Dzblart;
            model.Amount = this.Dmbtr;
            model.TransactionDescription = this.Sgtxt;
            model.LineText = this.Linetext;
            model.ChequeBounceStatus = this.Bouncestatus;
            model.BankNo = this.Bank;
            model.ChequeNo = this.Chequeno;
            return model;
        }
    }

}
