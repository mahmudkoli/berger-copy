using Berger.Data.Common;
using System.Collections.Generic;

namespace Berger.Data.MsfaEntity.Scheme
{
    public class SchemeMaster : AuditableEntity<int>
    {
        public string SchemeName { get; set; }
        public string Condition { get; set; }
        public IList<SchemeDetail> SchemeDetails { get; set; }
    }
}
