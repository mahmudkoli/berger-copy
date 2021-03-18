using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Odata.Model
{
    public class CustomerDataModel
    {
        public int CustomerNo { get; set; }
        public int Division { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; } // Area
        public string DayLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public string CustomerName { get; set; }
        public string CustZone { get; set; } // Zone
        public string BusinessArea { get; set; } // Plant, Depot
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string CreditControlArea { get; set; }
        public string CreatedOn { get; set; }
        public string CustomerGroup { get; set; }
        public string District { get; set; }
        public string PriceGroup { get; set; }
        public string SalesOrg { get; set; }
        public string Channel { get; set; }
    }
}
