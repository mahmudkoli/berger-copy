using System.ComponentModel.DataAnnotations.Schema;
using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;

namespace Berger.Data.MsfaEntity.Menus
{
    public class MenuPermission : AuditableEntity<int>
    {
        public int? RoleId { get; set; }
        public int MenuId { get; set; }
        public TypeEnum Type { get; set; }
        public EnumEmployeeRole? EmpRoleId { get; set; }


        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
    }
}
