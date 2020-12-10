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
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string  Territory { get; set; }
        public string Zone { get; set; }
        public string EmployeeId { get; set; }
        public List<DealerOpeningAttachment> DealerOpeningAttachments { get; set; }
    }
}
