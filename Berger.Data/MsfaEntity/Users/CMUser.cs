using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{

    public class CMUser : AuditableEntity<int>
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(128)]
        public string PhoneNumber { get; set; }
        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(256)]
        public string Password { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string FamilyContactNo { get; set; }
        
        public int? FMUserId { get; set; }

        [ForeignKey("FMUserId")]
        public UserInfo UserInfo { get; set; }
    }


}
