using Berger.Data.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Berger.Data.MsfaEntity.Scheme
{
    public class SchemeMaster : AuditableEntity<int>
    {
        public string SchemeName { get; set; }
        public string Condition { get; set; }
        [StringLength(100)]
        public string BusinessArea { get; set; }
        public IList<SchemeDetail> SchemeDetails { get; set; }
    }
}
