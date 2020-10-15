using Berger.Data.Common;
using Berger.Data.MsfaEntity.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerFocus
{
    public class DealerOpening:AuditableEntity<int>
    {
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string  TerritoryNo { get; set; }
        public string ZoneNo { get; set; }
        public string EmployeId { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
