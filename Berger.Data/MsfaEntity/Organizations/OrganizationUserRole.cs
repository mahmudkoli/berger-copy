using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;

namespace Berger.Data.MsfaEntity.Organizations
{

    public class OrganizationUserRole : AuditableEntity<int>
    {
        public int OrgRoleId { get; set; }

        [ForeignKey("OrgRoleId")]
        public OrganizationRole OrgRole { get; set; }

        public int UserId { get; set; }
        public int UserSequence { get; set; }
        [ForeignKey("UserId")]
        public UserInfo UserInfo { get; set; }

        public int DesignationId { get; set; }

    }

}
