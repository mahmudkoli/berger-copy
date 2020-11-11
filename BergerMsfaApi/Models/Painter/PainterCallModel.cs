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
        public bool HasSchemeComnunaction { get; set; }
        public bool HasPremiumProtBriefing { get; set; }
        public bool HasNewProBriefing { get; set; }
        public bool HasUsageEftTools { get; set; }
        public bool HasAppUsage { get; set; }
        public decimal WorkInHandNumber { get; set; }
        public bool HasDbblIssue { get; set; }
        public string Comment { get; set; }
        public int PainterId { get; set; }
        public List<PainterCompanyMTDValueModel> PainterCompanyMTDValueModel { get; set; }
    }
}
