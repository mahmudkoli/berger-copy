using System;
using Berger.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Scheme
{
    public class SchemeDetail : AuditableEntity<int>
    {
        //National Scheme (Brand)
        public string Code { get; set; }
        public string Brand { get; set; }
        public string RateInLtrOrKg { get; set; }
        public string RateInSKU { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        public string BenefitDate { get; set; }



        //Common
        public DateTime BenefitStartDate { get; set; }
        public DateTime? BenefitEndDate { get; set; }

        public string BusinessArea { get; set; }
        public SchemeType SchemeType { get; set; }
        public string SchemeName { get; set; }


        //public int SchemeMasterId { get; set; }
        //[ForeignKey("SchemeMasterId")]
        //  public SchemeMaster SchemeMaster { get; set; }
    }

    public enum SchemeType
    {
        National = 1,
        Regional = 2
    }

}
