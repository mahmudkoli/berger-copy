using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Users
{
   public class UserZoneAreaMapping:AuditableEntity<int>
    {
        public string PlantId { get; set; } // BusinessArea, Depot
        public string SalesOfficeId { get; set; }
        public string AreaId { get; set; } // SalesGroup
        public string TerritoryId { get; set; }
        public string ZoneId { get; set; } // CustZone

        [ForeignKey("UserInfoId")]
        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
