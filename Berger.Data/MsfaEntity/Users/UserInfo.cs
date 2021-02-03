using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Users
{
    public class UserInfo : AuditableEntity<int>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string EmployeeId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }

        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }

        public List<UserRoleMapping> Roles { get; set; }
        public List<UserZoneAreaMapping> UserZoneAreaMappings { get; set; }

        public UserInfo()
        {
            this.UserZoneAreaMappings = new List<UserZoneAreaMapping>();
        }
    }
}
