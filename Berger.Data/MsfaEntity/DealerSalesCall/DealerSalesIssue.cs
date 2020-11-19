using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
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

        public int DealerSalesIssueCategoryId { get; set; }
        public DropdownDetail DealerSalesIssueCategory { get; set; }
        public string MaterialName { get; set; }
        public string MaterialGroup { get; set; }
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public string Comments { get; set; }
        //public EnumPriority Priority { get; set; }
        public int PriorityId { get; set; }
        public DropdownDetail Priority { get; set; }

        public bool HasCBMachineMantainance { get; set; }
        //public bool IsCBMachineMantainanceRegular { get; set; }
        public int? CBMachineMantainanceId { get; set; }
        public DropdownDetail CBMachineMantainance { get; set; }
        public string CBMachineMantainanceRegularReason { get; set; }
    }
}
