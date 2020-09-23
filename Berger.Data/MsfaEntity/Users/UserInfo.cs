using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{
    public class UserInfo : AuditableEntity<int>
    {

        //[Required]
        [StringLength(128)]
        public string EmployeeId { get; set; }

        [UniqueKey]
        // [Required]
        [StringLength(128)]
        public string Code { get; set; }

        //[Required]
        [StringLength(256)]
        public string Name { get; set; }

        //[StringLength(256)]
        //public string BanglaName { get; set; }
        public string AdGuid { get; set; }

        public string email { get; set; }
        public string Groups { get; set; }

        [StringLength(128)]
        public string PhoneNumber { get; set; }


        [StringLength(256)]
        public string Designation { get; set; }// Is it int or string
        public int HierarchyId { get; set; }

        public List<UserRoleMapping> Roles { get; set; }
        public List<CMUser> CMUsers { get; set; }
        public List<UserTerritoryMapping> Territories { get; set; }
    }


}
