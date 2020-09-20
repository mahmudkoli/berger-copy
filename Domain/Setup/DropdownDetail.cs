using BergerMsfaApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BergerMsfaApi.Domain.Setup
{
    public class DropdownDetail : AuditableEntity<int>
    {
        [Column(Order = 1)]
        public int TypeId { get; set; }
        [StringLength(128, MinimumLength = 1)]
        public string DropdownName { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }

        [ForeignKey("TypeId")]
        public DropdownType DropdownType { get; set; }
    }
}
