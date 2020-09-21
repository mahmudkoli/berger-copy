using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.MsfaEntity.Menus;
using Berger.Data.MsfaEntity.Users;

namespace BergerMsfaApi.Models.Menus
{
    public class MenuActivityPermissionModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public MenuActivity Activity { get; set; }


        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanInsert { get; set; }
        public bool CanDelete { get; set; }
    }
}
