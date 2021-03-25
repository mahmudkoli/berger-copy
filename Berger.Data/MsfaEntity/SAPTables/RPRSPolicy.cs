using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class RPRSPolicy : AuditableEntity<int>
    {
        public int FromDaysLimit { get; set; }
        public int ToDaysLimit { get; set; }
        public int RPRSDays { get; set; }
        public int NotificationDays { get; set; }
    }
}
