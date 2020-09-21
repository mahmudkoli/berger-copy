using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Domain.CollectionEntry
{
    public class Payment:AuditableEntity<int>
    {

        public string PaymentFrom { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string SapId { get; set; }
        public string BankName { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }

        public string ManualNumber { get; set; }
        public string Remarks { get; set; }
        public int PaymentMethodId { get; set; }
        public int CreditControllAreaId { get; set; }

        [ForeignKey("PaymentMethodId")]
        public DropdownDetail PaymentMethod{ get; set; }

        [ForeignKey("CreditControllAreaId")]
        public DropdownDetail  CreditControllArea { get; set; }
    }

}
