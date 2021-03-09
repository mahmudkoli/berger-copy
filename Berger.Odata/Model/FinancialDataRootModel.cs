using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class FinancialDataRootModel
    {
        public string Companycode { get; set; }
        public string CustomerLow { get; set; }
        public string CustomerHigh { get; set; }
        public string Keydate { get; set; }
        public string Noteditems { get; set; }
        public string Secindex { get; set; }
        public string Mandt { get; set; }
        public string Bukrs { get; set; }
        public string Kunnr { get; set; }
        public string Name1 { get; set; }
        public string Vbeln { get; set; }
        public string Belnr { get; set; }
        public string Gjahr { get; set; }
        public string Dmbtr { get; set; }
        public string Age { get; set; }
        public string Daylimit { get; set; }
        public string BlineDate { get; set; }
        public string Budat { get; set; }
        public string Kkber { get; set; }

        public FinancialDataModel ToModel()
        {
            var model = new FinancialDataModel();
            model.CompanyCode = this.Companycode;
            model.FromCustomer = this.CustomerLow;
            model.ToCustomer = this.CustomerHigh;
            model.Date = this.Keydate;
            model.Noteditems = this.Noteditems;
            model.Secindex = this.Secindex;
            model.Mandt = this.Mandt;
            model.CompanyCode1 = this.Bukrs;
            model.CustomerNo = this.Kunnr;
            model.CustomerName = this.Name1;
            model.InvoiceNo = this.Vbeln;
            model.AccDocNo = this.Belnr;
            model.FiscalYear = this.Gjahr;
            model.Amount = this.Dmbtr;
            model.Age = this.Age;
            model.Daylimit = this.Daylimit;
            model.DueDate = this.BlineDate;
            model.PostingDate = this.Budat;
            model.CreditControlArea = this.Kkber;
            return model;
        }
    }

}
