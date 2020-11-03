using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Scheme
{
   public class SchemeBenefit:AuditableEntity<int>
    {
        public string Codition { get; set; }
        public string Benifit { get; set; }
        public string TargetVolum { get; set; }
        public DateTime Date { get; set; }

       
        public int SchemeDeatailId { get; set; }
        [ForeignKey("SchemeDeatailId")]
        public SchemeDetail SchemeDetail { get; set; }

        //public int SchemeMasterId { get; set; }
        //[ForeignKey("SchemeMasterId")]
        //public SchemeMaster SchemeMaster { get; set; }
      

    }
}
