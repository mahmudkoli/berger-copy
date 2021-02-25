using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class FinanceDateRootModel
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

        public FinanceDataModel ToModel()
        {
            return new FinanceDataModel();
        }
    }

}
