using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.KPI
{
    public class UniverseReachAnalysis : AuditableEntity<int>
    {
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public string FiscalYear { get; set; }
        public int OutletNumber { get; set; }
        public int DirectCovered { get; set; }
        public int IndirectCovered { get; set; }
        public int DirectTarget { get; set; }
        public int IndirectTarget { get; set; }
        //public int DirectActual { get; set; }
        //public int IndirectActual { get; set; }
        public int IndirectManual { get; set; }
    }
}
