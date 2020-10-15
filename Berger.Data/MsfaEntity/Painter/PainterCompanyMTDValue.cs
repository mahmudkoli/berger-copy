using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class PainterCompanyMTDValue:AuditableEntity<int>
    {
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public DropdownDetail Company { get; set; }
        public decimal Value { get; set; }
        public float CountInPercent { get; set; }
        public int PainterCallId { get; set; }

        [ForeignKey("PainterCallId")]
        public PainterCall PainterCall { get; set; }
    }
}
