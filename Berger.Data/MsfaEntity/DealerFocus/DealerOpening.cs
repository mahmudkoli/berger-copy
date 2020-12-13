using Berger.Data.Common;
using Berger.Data.MsfaEntity.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerFocus
{
    public class DealerOpening:AuditableEntity<int>
    {
        public DealerOpening()
        {
            DealerOpeningAttachments=new  List<DealerOpeningAttachment>();
        }
        public string BusinessArea { get; set; }
        public string SaleOfficeCd { get; set; }
        public string SaleGroupCd { get; set; }
        public string  TerritoryNoCd { get; set; }
        public string ZoneNoCd { get; set; }
        public int EmployeId { get; set; }
        public int? LineManagerId { get; set; }
        public List<DealerOpeningAttachment> DealerOpeningAttachments { get; set; }
    }
}
