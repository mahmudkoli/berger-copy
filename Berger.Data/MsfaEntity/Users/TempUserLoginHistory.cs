using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Users
{
   public class TempUserLoginHistory : Entity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public string JwtToken { get; set; }
        public bool FromAppLogin { get; set; }
        public DateTime LoggedInTime { get; set; }
        public string AppVersion { get; set; }
    }
}
