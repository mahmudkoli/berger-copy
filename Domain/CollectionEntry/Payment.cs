using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Domain.CollectionEntry
{
    public class Payment:AuditableEntity<int>
    {

        public string PaymentForm { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public int SAPID { get; set; }
        public string BankName { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }

        public string ManualNumber { get; set; }
        public string Remarks { get; set; }

        public DropdownDetail PaymentMethod{ get; set; }
        public DropdownDetail  CreditControlArea { get; set; }
    }

}
