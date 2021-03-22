using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.EmailLog
{
   public class EmailLog: AuditableEntity<int>
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachmenturl { get; set; }
        public int LogStatus { get; set; }
        public string FailResoan { get; set; }
    }
}
