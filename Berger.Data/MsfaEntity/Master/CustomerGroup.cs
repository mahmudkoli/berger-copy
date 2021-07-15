using Berger.Data.Common;
using System;
using System.Collections.Generic;

namespace Berger.Data.MsfaEntity.Master
{
    public class CustomerGroup : Entity<int>
    {
        public string CustomerAccountGroup { get; set; }
        public string Description { get; set; }

        public bool IsSubdealer()
        {
            return this != null && !string.IsNullOrEmpty(this.Description) && this.Description.StartsWith("Subdealer");
        }
    }
}
