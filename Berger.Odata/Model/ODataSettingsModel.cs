using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model
{
    public class ODataSettingsModel
    {
        public string BaseAddress { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string SalesUrl { get; set; }
        public string DriverUrl { get; set; }
        public string MTSUrl { get; set; }
        public string BrandFamilyUrl { get; set; }
        public string FinancialUrl { get; set; }
        public string BalanceUrl { get; set; }
        public string CollectionUrl { get; set; }
        public string CustomerUrl { get; set; }
        public string StockUrl { get; set; }
        public string CustomerOccasionUrl { get; set; }
        public string CustomerCreditUrl { get; set; }
        public string CustomerDeliveryUrl { get; set; }
    }
}
