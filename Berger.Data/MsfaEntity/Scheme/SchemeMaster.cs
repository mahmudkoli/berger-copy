using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.Scheme
{
    public class SchemeMaster : AuditableEntity<int>
    {
        public string   SchemeName { get; set; }
        public string   Condition { get; set; }
        public List<SchemeDetail> SchemeDetail { get; set; }
       
    }
}
