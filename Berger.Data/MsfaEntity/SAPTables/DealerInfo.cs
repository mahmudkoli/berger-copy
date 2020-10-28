﻿
using Berger.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class DealerInfo : AuditableEntity<int>
    {
        //public int Id { get; set; }
        public int CustomerNo { get; set; }

        public int Division { get; set; }
        public string DayLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
  
        public string CustomerName { get; set; }
        public string CustZone { get; set; }
        public string BusinessArea { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string CreditControlArea { get; set; }
    }
}
