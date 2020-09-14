using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Models.Organizations
{
    public class OrganizationRoleModel
    {
        public OrganizationRoleModel()
        {
            this.ConfigList = new List<OrganizationRoleMappingModel>();
            this.UserList = new List<int>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public Status Status { get; set; }
        public int DesignationId { get; set; }
        public List<int> UserList { get; set; }
        public int? WorkflowId { get; set; }
        public WFStatus WFStatus { get; set; }
        public List<OrganizationRoleMappingModel> ConfigList;
    }
}
