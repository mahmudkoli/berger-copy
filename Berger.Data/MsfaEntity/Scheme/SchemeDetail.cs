using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Scheme
{
   public class SchemeDetail:AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Slab { get; set; }
        public string Item { get; set; }
        public string Condition { get; set; }
        public string TargetVolume { get; set; }
        public string Benefit { get; set; }

        public string Date { get; set; }
        public int SchemeMasterId { get; set; }

        [ForeignKey("SchemeMasterId")]
        public SchemeMaster SchemeMaster { get; set; }


  
    }
}
