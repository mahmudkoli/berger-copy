using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class ELearningDocument : AuditableEntity<int>
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DropdownDetail Category { get; set; }
        public IList<ELearningAttachment> ELearningAttachments { get; set; }
    }
}
