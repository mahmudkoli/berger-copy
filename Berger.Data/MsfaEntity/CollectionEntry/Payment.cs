
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.CollectionEntry
{
    public class Payment:AuditableEntity<int>
    {
        public int CustomerTypeId { get; set; }

        [ForeignKey("CustomerTypeId")]
        public DropdownDetail CustomerType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CollectionDate { get; set; }
        public string DealerId { get; set; } // DealerId for dealer/sub dealer
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string SapId { get; set; } // Direct Project
        public string BankName { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }

        public string ManualNumber { get; set; }
        public string Remarks { get; set; }
        public int? PaymentMethodId { get; set; }
        

        [ForeignKey("PaymentMethodId")]
        public DropdownDetail PaymentMethod{ get; set; }
        public int CreditControlAreaId { get; set; }
        [ForeignKey("CreditControlAreaId")]
        public CreditControlArea CreditControlArea { get; set; }
        public string EmployeeId { get; set; }
    }

}
