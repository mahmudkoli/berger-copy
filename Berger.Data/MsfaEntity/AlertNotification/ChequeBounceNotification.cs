using System;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class ChequeBounceNotification
    {
        public Guid Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomarNo { get; set; }
        public string CustomerName { get; set; }
        public string ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
