using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.KPI
{
    public class CollectionPlan : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        //public decimal SlippageAmount { get; set; }
        public decimal CollectionTargetAmount { get; set; }
        //public decimal CollectionActualAmount { get; set; }
        //public decimal SlippageCollectionActualAmount { get; set; }
    }
}
