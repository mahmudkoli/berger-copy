using System;
using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.CollectionEntry
{
    public class PaymentModel
    {
        public int Id { get; set; }
        [Required]


        public string CollectionDate { get; set; }
        public int CustomerTypeId { get; set; }
        public string DealerId { get; set; }
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

        public int CreditControlAreaId { get; set; }
        public string CreditControlAreaName { get; set; }
        public string EmployeeId { get; set; }
    }

    public class AppCollectionEntryModel
    {
        public int Id { get; set; }
        public string CollectionDate { get; set; }
        public string CustomerType { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditControlArea { get; set; }
    }
}
