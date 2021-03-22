using Berger.Data.Common;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerFocus
{
   public class DealerOpeningLog: AuditableEntity<int>
    {
        public int UserId { get; set; }
        public int DealerOpeningId { get; set; }
        public DealerOpening DealerOpening { get; set; }

        public UserInfo User { get; set; }

        public string PropertyValue { get; set; }
        public string PropertyName { get; set; }
    }
}
