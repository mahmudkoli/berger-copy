using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class DealerInfo : AuditableEntity<int>
    {
    
        public int Id { get; set; }
        public int CustomerNo { get; set; }
        public int DivisionId { get; set; }
        public string DayLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public string CustomerName { get; set; }
        public string CustomerZone { get; set; }
        public string BusinessArea { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string CreditControlArea { get; set; }
        private string CompoKey;
        [NotMapped] 
        public string CompositeKey
        {
            get => CustomerNo.ToString() + AccountGroup + BusinessArea;
            set => CompoKey = value;
        }
    }
}
