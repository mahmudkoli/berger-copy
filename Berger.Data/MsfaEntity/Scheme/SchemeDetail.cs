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
        public string RateInDrum { get; set; }

        //National Scheme (Value)
        public string Slab { get; set; }
        public string Condition { get; set; }
        public string BenefitDate { get; set; }

        //Painter Scheme
        public string SchemeId { get; set; }
        public string Material { get; set; }
        public string TargetVolume { get; set; }

        //Common
        public string Benefit { get; set; }
        public DateTime BenefitStartDate { get; set; }
        public DateTime? BenefitEndDate { get; set; }
        public int SchemeMasterId { get; set; }
        [ForeignKey("SchemeMasterId")]
        public SchemeMaster SchemeMaster { get; set; }
    }
}
