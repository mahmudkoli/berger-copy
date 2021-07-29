using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.SAPTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DemandGeneration
{
    public class LeadActualVolumeSold : Entity<int>
    {
        public int LeadFollowUpId { get; set; }
        public LeadFollowUp LeadFollowUp { get; set; }
        public int BrandInfoId { get; set; }
        public BrandInfo BrandInfo { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumLeadActualVolumeSoldType ActualVolumeSoldType { get; set; }
    }
}
