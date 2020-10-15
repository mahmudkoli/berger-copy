using Berger.Data.MsfaEntity.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCallModel
    {
        public int Id { get; set; }
        public bool SchemeComnunaction { get; set; }
        public bool PremiumProtBriefing { get; set; }
        public bool NewProBriefing { get; set; }
        public bool UsageEftTools { get; set; }
        public bool AppUsage { get; set; }
        public string Number { get; set; }
        public int PainterId { get; set; }
        public List<PainterCompanyMTDValueModel> PainterCompanyMTDValueModel { get; set; }
    }
}
