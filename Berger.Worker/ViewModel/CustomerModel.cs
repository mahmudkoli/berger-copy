using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Worker.ViewModel
{
    class CustomerModel
    {
        public int CustomerNo { get; set; }
        public int DivisionId { get; set; }
        public string DayLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public string CustomerName { get; set; }
        public string CustZone { get; set; }
        public string BusinessArea { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string CreditControlArea { get; set; }
        private string CompoKey;
       
        public string CompositeKey
        {
            get => CustomerNo.ToString() + AccountGroup + BusinessArea;
            set => CompoKey = value;
        }
        //public Object __metadata { get; set; }

    }
}
