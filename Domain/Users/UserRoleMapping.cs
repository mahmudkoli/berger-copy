using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Core;

namespace BergerMsfaApi.Domain.Users
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
