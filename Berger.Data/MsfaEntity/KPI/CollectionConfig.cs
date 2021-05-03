using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.KPI
{
    public class CollectionConfig : AuditableEntity<int>
    {
        public int ChangeableMaxDateDay { get; set; }
    }
}
