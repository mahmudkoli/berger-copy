using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Users;

namespace BergerMsfaApi.Domain.Menus
{
    public class MenuPermission : AuditableEntity<int>
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
    }
}
