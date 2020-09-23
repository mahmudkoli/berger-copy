using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Common.Enumerations;

namespace BergerMsfaApi.Models.Users
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {
            this.TerritoryNodeIds = new List<int>();
            this.AreaNodeIds = new List<int>();
            this.RegionNodeIds = new List<int>();
            this.NationalNodeIds = new List<int>();
        }

        public int Id { get; set; }

      //  [Required]
        [StringLength(128)]
        public string EmployeeId { get; set; }

       // [Required]
        [StringLength(128)]
        public string Code { get; set; }

       // [Required]
        [StringLength(256)]
        public string Name { get; set; }


        [StringLength(128)]
        public string PhoneNumber { get; set; }


        [StringLength(256)]
        public string Designation { get; set; }// Is it int or string

        public int HierarchyId { get; set; }
        public int RoleId { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> NationalNodeIds {get; set;}
        public List<int> TerritoryNodeIds { get; set; }
        public List<int> AreaNodeIds { get; set; }
        public List<int> RegionNodeIds { get; set; }
        public Status Status { get; set; }

        public int MyProperty { get; set; }
        public int NodeId { get; set; }


        [StringLength(128)] 
        public string Email { get; set; }
        [StringLength(128)]
        public string Groups { get; set; }

        public string AdGuid { get; set; }
        public string RoleName { get; set; }
    }
}
