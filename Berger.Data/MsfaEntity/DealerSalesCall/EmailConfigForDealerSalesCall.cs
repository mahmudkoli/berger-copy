using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
   public class EmailConfigForDealerSalesCall: AuditableEntity<int>
    {
        public string BusinessArea { get; set; }
        public int DealerSalesIssueCategoryId { get; set; }
        public DropdownDetail DealerSalesIssueCategory { get; set; }
        public string Email { get; set; }
    }
}
