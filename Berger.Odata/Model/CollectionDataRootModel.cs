using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class CollectionDataRootModel
    {
        public string Company { get; set; }
        public string SpecialGl { get; set; }
        public string TransactionDetail { get; set; }
        public string docNumber { get; set; }
        public string FiscalYear { get; set; }
        public string docType { get; set; }
        public string BusinessArea { get; set; }
        public string PaymentTerm { get; set; }
        public string BillNo { get; set; }
        public string ltext { get; set; }
        public string chqNo { get; set; }
        public string AccountGroup { get; set; }
        public string bank { get; set; }
        public string stgrd { get; set; }
        public string stblg { get; set; }
        public string stjah { get; set; }
        public string blart { get; set; }
        public string Amount { get; set; }
        public string AccountGroupName { get; set; }
        public string Customer { get; set; }
        public string CustomerName { get; set; }
        public string rpmkr { get; set; }
        public string Depot { get; set; }
        public string PostingDate { get; set; }
        public string augdt { get; set; }
        public string CreditControlArea { get; set; }

        public CollectionDataModel ToModel()
        {
            var model = new CollectionDataModel();
            model.Company = this.Company;
            model.SpecialGl = this.SpecialGl;
            model.TransactionDetail = this.TransactionDetail;
            model.DocNumber = this.docNumber;
            model.FiscalYear = this.FiscalYear;
            model.DocType = this.docType;
            model.BusinessArea = this.BusinessArea;
            model.PaymentTerm = this.PaymentTerm;
            model.BillNo = this.BillNo;
            model.LineText = this.ltext;
            model.ChequeNo = this.chqNo;
            model.AccountGroup = this.AccountGroup;
            model.BankName = this.bank;
            model.BounceStatus = this.stgrd;
            model.stblg = this.stblg;
            model.stjah = this.stjah;
            model.CollectionType = this.blart;
            model.Amount = this.Amount;
            model.AccountGroupName = this.AccountGroupName;
            model.CustomerNo = this.Customer;
            model.CustomerName = this.CustomerName;
            model.Territory = this.rpmkr;
            model.Depot = this.Depot;
            model.PostingDate = this.PostingDate;
            model.ClearDate = this.augdt;
            model.CreditControlArea = this.CreditControlArea;
            return model;
        }
    }

}
