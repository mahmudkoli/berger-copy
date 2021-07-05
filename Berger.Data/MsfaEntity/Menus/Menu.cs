using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Common.Enumerations;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Menus
{
    public class Menu : AuditableEntity<int>
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Controller { get; set; }

        [StringLength(128)]
        public string Action { get; set; }

        [StringLength(256)]
        public string Url { get; set; }

        [StringLength(128)]
        public string IconClass { get; set; }

        public int ParentId { get; set; }
        public bool IsParent { get; set; }
        public bool IsTitle { get; set; }
        public int Sequence { get; set; }
        public TypeEnum Type { get; set; }
        public string Code { get; set; }

        public List<MenuPermission> MenuPermissions { get; set; }
        public List<MenuActivity> MenuActivities { get; set; }
    }
}
