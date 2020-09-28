using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Berger.Data.MsfaEntity.Setup
{
    public class DropdownType : AuditableEntity<int>
    {
        public DropdownType()
        {
            this.DropdownDetails = new List<DropdownDetail>();
        }
        [StringLength(128, MinimumLength = 1)]
        public string TypeName { get; set; }

        public string TypeCode { get; set; }
        public List<DropdownDetail> DropdownDetails { get; set; }
    }
}
