using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Examples
{
    public class Example : AuditableEntity<int>
    {
        [UniqueKey(groupId: "1", order: 0)]
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

    }
}
