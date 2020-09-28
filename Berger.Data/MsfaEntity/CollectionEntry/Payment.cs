
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.CollectionEntry
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
        public int? PaymentMethodId { get; set; }
        

        [ForeignKey("PaymentMethodId")]
        public DropdownDetail PaymentMethod{ get; set; }
        public int CreditControllAreaId { get; set; }

        [ForeignKey("CreditControllAreaId")]
        public DropdownDetail  CreditControllArea { get; set; }
    }

}
