using System;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class PaymentFollowupNotification
    {
        public Guid Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomarNo { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? PostingDate { get; set; }
        public int InvoiceAge { get; set; }
        public int DayLimit { get; set; }
        public string PriceGroup { get; set; }
        public DateTime NotificationDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public bool IsRprsPayment { get; set; }
        public bool IsFastPayCarryPayment { get; set; }

    }
}
