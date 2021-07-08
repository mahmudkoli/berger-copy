﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class PaymentFollowup
    {
        public int Id { get; set; }
        public string Depot { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string Division { get; set; }
        public string DissChannel { get; set; }
        public string CustomarNo { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAge { get; set; }
        public string DayLimit { get; set; }
    }
}