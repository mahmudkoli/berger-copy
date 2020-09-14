using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Users;

namespace BergerMsfaApi.Domain.Organizations
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
