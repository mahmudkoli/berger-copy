using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{

    public class UserRoleMapping :AuditableEntity<int>
    {      
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

       
        public int UserInfoId { get; set; }


        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }      
    }
}
