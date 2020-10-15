using Berger.Data.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class PainterCall:AuditableEntity<int>
    {
        public bool SchemeComnunaction { get; set; }
        public bool PremiumProtBriefing { get; set; }
        public bool NewProBriefing { get; set; }
        public bool UsageEftTools { get; set; }
        public bool AppUsage { get; set; }
        public string Number { get; set; }
        public int PainterId { get; set; }
        [ForeignKey("PainterId")]
        public Painter Painter { get; set; }
        public List<PainterCompanyMTDValue> PainterCompanyMTDValue { get; set; }
    }
}
