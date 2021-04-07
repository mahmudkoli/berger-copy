using Berger.Data.Common;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
   public class MerchandisingSnapShot : AuditableEntity<int>
    {
        public int DealerId { get; set; }
        public DealerInfo Dealer { get; set; }
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public int MerchandisingSnapShotCategoryId { get; set; }
        public DropdownDetail MerchandisingSnapShotCategory { get; set; }
        public string OthersSnapShotCategoryName { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }
    }
}
