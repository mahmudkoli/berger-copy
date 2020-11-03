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
        public string Product { get; set; }
        [ForeignKey("SchemeMasterId")]
        public SchemeMaster SchemeMaster { get; set; }
        public int SchemeMasterId { get; set; }
    }
}
