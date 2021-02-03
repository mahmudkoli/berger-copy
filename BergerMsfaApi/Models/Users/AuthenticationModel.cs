using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Users
{
    public class AuthenticateUserModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public List<string> PlanIds { get; set; }
        public string PlanId { get; set; }
        public List<string> SalesOfficeIds { get; set; }
        public string SalesOfficeId { get; set; }
        public List<string> AreaIds { get; set; }
        public string AreaId { get; set; }
        public List<string> TerritoryIds { get; set; }
        public string TerritoryId { get; set; }
        public List<string> ZoneIds { get; set; }
        public string ZoneId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string EmployeeId { get; set; }
        public string UserCategory { get; set; }
        public List<string> UserCategoryIds { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
