using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.Users;
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
            dealerOpeningLogs = new List<DealerOpeningLog>();
        }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string  Territory { get; set; }
        public string Zone { get; set; }
        public string EmployeeId { get; set; }
        public int? CurrentApprovarId { get; set; }
        public int? NextApprovarId { get; set; }
        public UserInfo CurrentApprovar { get; set; }
        public UserInfo NextApprovar { get; set; }
        public string Comment { get; set; }
        public int DealerOpeningStatus { get; set; }


        public List<DealerOpeningAttachment> DealerOpeningAttachments { get; set; }
        public List<DealerOpeningLog> dealerOpeningLogs { get; set; }

    }
}
