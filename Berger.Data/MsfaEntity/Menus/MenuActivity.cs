using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Menus
{
    public class MenuActivity : AuditableEntity<int>
    {
        [UniqueKey()]
        [Column(Order = 1)]
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [UniqueKey()]
        [Column(Order = 2)]
        public string ActivityCode { get; set; }

        [Column(Order = 3)]
        [ForeignKey("MenuId")]
        public int MenuId { get; set; }
        
        public Menu Menu { get; set; }
        public List<MenuActivityPermission> Permissions { get; set; }

    }
}
