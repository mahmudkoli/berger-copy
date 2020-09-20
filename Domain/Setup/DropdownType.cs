using BergerMsfaApi.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Domain.Setup
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
