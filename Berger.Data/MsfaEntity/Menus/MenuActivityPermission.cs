using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Attributes;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;

namespace Berger.Data.MsfaEntity.Menus
{
    public class MenuActivityPermission : AuditableEntity<int>
    {
        [UniqueKey(groupId: "1", order: 0)]
        [Column(Order = 1)]
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        [UniqueKey(groupId: "1", order: 1)]
        [Column(Order = 2)]
        [ForeignKey("ActivityId")]
        public int ActivityId { get; set; }

        [Column(Order = 3)]
        public bool CanView { get; set; }

        [Column(Order = 4)]
        public bool CanUpdate { get; set; }

        [Column(Order = 5)]
        public bool CanInsert { get; set; }

        [Column(Order = 6)]
        public bool CanDelete { get; set; }

        public Role Role { get; set; }
        public MenuActivity Activity { get; set; }
    }


}
