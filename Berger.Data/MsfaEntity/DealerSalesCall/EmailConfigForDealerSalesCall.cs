using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
   public class EmailConfigForDealerSalesCall: AuditableEntity<int>
    {
        public int DealerSalesIssueCategoryId { get; set; }
        public string Email { get; set; }
    }
}
