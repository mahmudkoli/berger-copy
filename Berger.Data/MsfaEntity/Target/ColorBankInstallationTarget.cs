using System.ComponentModel.DataAnnotations;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Target
{
    public class ColorBankInstallationTarget : AuditableEntity<int>
    {
        [StringLength(50)]
        public string BusinessArea { get; set; }
        [StringLength(50)]
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Target { get; set; }

    }
}
