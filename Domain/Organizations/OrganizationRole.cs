using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Attributes;
using BergerMsfaApi.Core;

namespace BergerMsfaApi.Domain.Organizations
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
