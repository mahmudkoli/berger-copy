using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Users
{
   public class UserZoneAreaMapping:AuditableEntity<int>
    {
        public int PlanId { get; set; }
        public string SalesOfficeId { get; set; }
        public string AreaId { get; set; }
        public string TerritoryId { get; set; }
        public string ZoneId { get; set; }

        [ForeignKey("UserInfoId")]
        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
