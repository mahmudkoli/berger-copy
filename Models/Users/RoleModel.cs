using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Models.Users
{
    public class RoleModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public Status Status { get; set; }


        public int? WorkflowId { get; set; }
        public WFStatus WFStatus { get; set; }
    }
}