using System.Collections.Generic;
using System.Security.Principal;

namespace BergerMsfaApi.Core
{
    public interface IAppUserPrincipal : IPrincipal
    {
        int UserId { get; set; }
        string UserName { get; }
        string FullName { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string EmployeeId { get; set; }
        string Avatar { get; set; }
        int ActiveRoleId { get; set; }
        List<int> RoleIdList { get; set; }
        string RoleIds { get; set; }
        string UserAgentInfo { get; set; }
        int NodeId { get; set; }
        int EmployeeRole { get; set; }
        List<string> PlantIdList { get; set; }
        string PlantIds { get; set; }
        List<string> SalesOfficeIdList { get; set; }
        string SalesOfficeIds { get; set; }
        List<string> SalesAreaIdList { get; set; }
        string SalesAreaIds { get; set; }
        List<string> TerritoryIdList { get; set; }
        string TerritoryIds { get; set; }
        List<string> ZoneIdList { get; set; }
        string ZoneIds { get; set; }
        Dictionary<string, string> GetByName();
    }
}
