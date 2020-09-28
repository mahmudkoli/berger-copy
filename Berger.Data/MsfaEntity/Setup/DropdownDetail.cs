using Berger.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Setup
{
    public class DropdownDetail : AuditableEntity<int>
    {
       
       
        public string DropdownName { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public DropdownType DropdownType { get; set; }
    }
}
