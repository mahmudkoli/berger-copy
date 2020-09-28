using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{

    public class UserTerritoryMapping : AuditableEntity<int>
    {
        public int NodeId { get; set; }

        //[ForeignKey("NodeId")]
        //public Node Territory { get; set; }


        public int UserInfoId { get; set; }


        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }
    }
}
