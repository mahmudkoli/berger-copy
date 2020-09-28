using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Organizations
{
    public class OrganizationRole : AuditableEntity<int>
    {
        [UniqueKey]
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
        public int DesignationId { get; set; }
        public List<OrganizationUserRole> Users { get; set; }

    }

}
