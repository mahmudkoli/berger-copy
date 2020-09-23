using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Menus;

namespace Berger.Data.MsfaEntity.Users
{
    public class Role : AuditableEntity<int>
    {
        [UniqueKey]
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
        public List<UserRoleMapping> Users { get; set; }
        public List<MenuPermission> Permissions { get; set; }

    }
}
