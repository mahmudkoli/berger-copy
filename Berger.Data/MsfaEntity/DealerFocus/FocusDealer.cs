using Berger.Data.Common;
using System;

namespace Berger.Data.MsfaEntity.DealerFocus
{
    public class FocusDealer:AuditableEntity<int>
    {
        public int Code { get; set; }
        public string EmployeeId { get; set; }
     
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
