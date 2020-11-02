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

            plantIds = new List<int>();
            RoleIds = new List<int>();
            areaIds = new List<int?>();
            zoneIds = new List<int?>();
            saleOfficeIds = new List<int?>();
            territoryIds = new List<int?>();




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
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [StringLength(128)]
        public string MiddleName { get; set; }
        public string ManagerName { get; set; }
        public string LoginName { get; set; }
        public string LoginNameWithDomain { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public string StreetAddress { get; set; }
        public string Extension { get; set; }
        public string Fax { get; set; }
        public string StatusText { get; set; }

        public string State { get; set; }
        public int HierarchyId { get; set; }
        public int RoleId { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> NationalNodeIds { get; set; }
        public List<int> TerritoryNodeIds { get; set; }
        public List<int> AreaNodeIds { get; set; }
        public List<int> RegionNodeIds { get; set; }
        public List<int> plantIds { get; set; }
        public List<int?> areaIds { get; set; }
        public List<int?> zoneIds { get; set; }
        public List<int?> saleOfficeIds { get; set; }
        public List<int?> territoryIds { get; set; }
        public Status Status { get; set; }

        public int MyProperty { get; set; }
        public int NodeId { get; set; }

        public string Company { get; set; }
        public string Title { get; set; }

        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(128)]
        public string Groups { get; set; }

        public string AdGuid { get; set; }
        public string RoleName { get; set; }
    }
}


