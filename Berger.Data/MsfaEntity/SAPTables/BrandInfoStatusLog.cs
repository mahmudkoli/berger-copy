using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.SAPTables
{
   public class BrandInfoStatusLog : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public int BrandInfoId { get; set; }
        public BrandInfo BrandInfo { get; set; }
        
        public UserInfo User { get; set; }

        public string PropertyValue { get; set; }
        public string PropertyName { get; set; }

    }
}
