using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berger.Common.Enumerations;

namespace BergerMsfaApi.Models.Menus
{
    public class MenuActivityModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }
        public string  ActivityCode { get; set; }
        [ForeignKey("MenuId")]
        public int MenuId { get; set; }
        
        public MenuModel Menu { get; set; }
        public Status Status { get; set; }

        public MenuActivityPermissionModel MenuActivityPermission {get; set;}
        public List<MenuActivityPermissionModel> Permissions { get; set; } = new List<MenuActivityPermissionModel>();
    

    }
}
