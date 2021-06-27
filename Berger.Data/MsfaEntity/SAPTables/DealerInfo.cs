using System.ComponentModel.DataAnnotations.Schema;
using Berger.Common.Enumerations;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class DealerInfo : AuditableEntity<int>
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; } // Area
        public string DayLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public string CustomerName { get; set; }
        public string CustZone { get; set; } // Zone
        public string BusinessArea { get; set; } // Plant, Depot
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string CreditControlArea { get; set; }
        public string CreatedOn { get; set; }
        public string CustomerGroup { get; set; }
        public string District { get; set; }
        public string PriceGroup { get; set; }
        public string SalesOrg { get; set; }
        public string Channel { get; set; }
        public string CustomerClasification { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsLastYearAppointed { get; set; }
        public bool IsAP { get; set; }
        public EnumClubSupreme ClubSupremeType { get; set; }

        private string compositeKey;
        [NotMapped] 
        public string CompositeKey
        {
            //get => CustomerNo.ToString() + AccountGroup + BusinessArea;
            get => CustomerNo.ToString() + Channel + Division;
            set => compositeKey = value;
        }
    }
}
