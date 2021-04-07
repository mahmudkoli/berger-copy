using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.Users
{
   public class LoginLog : Entity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public string FCMToken { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LoggedInTime { get; set; }
        public DateTime? LoggedOutTime { get; set; }
    }
}
