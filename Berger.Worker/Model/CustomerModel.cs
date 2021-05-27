using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Worker.Model
{
    class CustomerModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
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

        //public bool IsDeleted { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsExclusive { get; set; }
        //public bool IsCBInstalled { get; set; }

        //[NotMapped]
        public string CreatedOn { get; set; }
        //[NotMapped]
        public string CustomerGroup { get; set; }
        //[NotMapped]
        public string District { get; set; }
        //[NotMapped]
        public string PriceGroup { get; set; }
        //[NotMapped]
        public string SalesOrg { get; set; }
        //[NotMapped]
        public string Channel { get; set; }
        //[NotMapped]
        public string CustomerClasification { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            //get => CustomerNo.ToString() + AccountGroup + BusinessArea;
            get => CustomerNo.ToString() + Channel + Division;
            set => compositeKey = value;
        }
    }
}
