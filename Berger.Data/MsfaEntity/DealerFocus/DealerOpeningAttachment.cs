using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.DealerFocus
{
   public class DealerOpeningAttachment:AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public int DealerOpeningId { get; set; }
    }
}
