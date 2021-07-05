using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Common.Enumerations;

namespace BergerMsfaApi.Models.Menus
{
    public class MenuModel
    {
        public int Id { get; set; }
        public Status Status { get; set; }

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

        public List<MenuPermissionModel> MenuPermissions { get; set; }
        public List<MenuActivityModel> MenuActivities { get; set; }
        public List<MenuModel> Children { get; set; }

    }
}
