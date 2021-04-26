using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Odata.Model
{
    public class CustomerCreditDataModel
    {
        public string Customer { get; set; }
        public string CreditControlArea { get; set; }
        public string Receivable { get; set; }
        public string OpenOrder { get; set; }
        public string OpenDelivery { get; set; }
        public string OpenBill { get; set; }
        public string CreditLimit { get; set; }
        public string LastPayment { get; set; }
        public string LastPaymentDate { get; set; }
    }
}
