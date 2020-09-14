using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Enumerations;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Models.Menus
{
    public class MenuPermissionModel
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        [ForeignKey("RoleId")]
        public RoleModel Role { get; set; }

        [ForeignKey("MenuId")]
        public MenuModel Menu { get; set; }
    }
}
