using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class CreditLimitCrossNotifiction
    {
        public int Id { get; set; }
        public string Depot { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public string PriceGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string Division { get; set; }
        public string DissChannel { get; set; }
        public string CustomarNo { get; set; }
        public string CustomerName { get; set; }
        public string CreditControlArea { get; set; }
        public string CreditLimit { get; set; }
        public string TotalDue { get; set; }
        public string Channel { get; set; }
        public DateTime NotificationDate { get; set; }

    }
}
