using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.CollectionEntry
{
    public class PaymentModel
    {
        public int Id { get; set; }
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
        public string PaymentMethodName { get; set; }

        public int CreditControllAreaId { get; set; }
        public string CreditControllAreaName { get; set; }
    }
}
