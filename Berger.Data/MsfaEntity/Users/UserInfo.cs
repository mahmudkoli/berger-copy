using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{
    public class UserInfo : AuditableEntity<int>
    {

        public UserInfo()
        {
            this.UserZoneAreaMappings = new List<UserZoneAreaMapping>();
        }
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

        //[Required]
        [StringLength(256)]
        public string UserName { get; set; }

        //[StringLength(256)]
        //public string BanglaName { get; set; }
        public string AdGuid { get; set; }

        public string Email { get; set; }
        public string Groups { get; set; }

        [StringLength(128)]
        public string PhoneNumber { get; set; }

        public string City { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string DepartMent { get; set; }
        public string Extension { get; set; }
        public string Fax { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
     

        public string LoginName { get; set; }
        public string LoginNameWithDomain { get; set; }

        public string Manager { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }
        public string MiddleName { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string StreetAddress { get; set; }
        public string Title { get; set; }

        [StringLength(256)]
        public string Designation { get; set; }// Is it int or string
        public int HierarchyId { get; set; }
        public int? LinemanagerId { get; set; }
        public List<UserHirearchyInfo> UserHirearchyInfos { get; set; }
        public List<UserRoleMapping> Roles { get; set; }
        public List<UserZoneAreaMapping> UserZoneAreaMappings { get; set; }
        public List<CMUser> CMUsers { get; set; }
        public List<UserTerritoryMapping> Territories { get; set; }
    }


}
