using Berger.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Users
{
    public class UserHirearchyInfo:AuditableEntity<int>
    {
        public string  Plan { get; set; }
        public int SaleOffice { get; set; }
        public int AreaGroup { get; set; }
        public string Territory { get; set; }
        public int Zone { get; set; }

        public int UserInfoId { get; set; }

        [ForeignKey("UserInfoId")]
        public UserInfo UserInfos { get; set; }
    }
}
