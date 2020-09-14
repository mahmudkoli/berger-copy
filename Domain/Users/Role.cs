using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Attributes;
using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Menus;

namespace BergerMsfaApi.Domain.Users
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
