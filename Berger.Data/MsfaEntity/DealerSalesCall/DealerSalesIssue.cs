using Berger.Common.Enumerations;
using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
    public class DealerSalesIssue : Entity<int>
    {
        public int DealerSalesCallId { get; set; }
        public DealerSalesCall DealerSalesCall { get; set; }

        public string MaterialName { get; set; }
        public string MaterialGroup { get; set; }
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public string Comments { get; set; }
        public EnumPriority Priority { get; set; }

        public bool IsCBMachineMantainance { get; set; }
        public bool IsCBMachineMantainanceRegular { get; set; }
        public string CBMachineMantainanceRegularReason { get; set; }
    }
}
