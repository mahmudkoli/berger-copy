using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.Setup
{
   public class EmailConfigForDealerOppening: AuditableEntity<int>
    {
        public string Designation { get; set; }
        public string Email { get; set; }
    }
}
