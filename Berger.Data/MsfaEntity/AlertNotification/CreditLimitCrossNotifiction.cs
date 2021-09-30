using System;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class CreditLimitCrossNotification
    {
        public Guid Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string PriceGroup { get; set; }
        public string CreditControlArea { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public DateTime NotificationDate { get; set; }

    }
}
