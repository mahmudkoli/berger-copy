using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace BergerMsfaApi.Core
{
    public class AppUserPrincipal : IAppUserPrincipal
    {
        public AppUserPrincipal(string userName)
        {
            Identity = new GenericIdentity(userName);
            UserName = userName; 
        }

        public AppUserPrincipal(string userName, string type)
        {
            Identity = new GenericIdentity(userName, type);
            UserName = userName;
        }

        public bool IsInRole(string roleId)
        {
            return RoleIdList.Any(m => m.ToString().Equals(roleId, StringComparison.CurrentCultureIgnoreCase));
        }

        public IIdentity Identity { get; set; }
        public int UserId { get; set; }
        public string UserName { get; private set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public string Avatar { get; set; }
        public int ActiveRoleId { get; set; }
        public string ActiveRoleName { get; set; }
        public List<int> RoleIdList { get; set; } = new List<int>();

        public string RoleIds
        {
            get => string.Join(",", RoleIdList.Select(s => (int)s));
            set
            {
                RoleIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => (int)(Convert.ToInt32(s.Trim()))).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<int> { (int)(Convert.ToInt32(value)) }
                        : new List<int> { };
            }
        }

        public string UserAgentInfo { get; set; }
        public int NodeId { get;  set; }
        public List<int> Successors { get; set; } = new List<int>();

        public int EmployeeRole { get; set; }
        public List<string> PlantIdList { get; set; } = new List<string>();
        public string PlantIds
        {
            get => string.Join(",", PlantIdList.Select(s => s));
            set
            {
                PlantIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<string> { value }
                        : new List<string> { };
            }
        }

        public List<string> SalesOfficeIdList { get; set; } = new List<string>();
        public string SalesOfficeIds
        {
            get => string.Join(",", SalesOfficeIdList.Select(s => s));
            set
            {
                SalesOfficeIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<string> { value }
                        : new List<string> { };
            }
        }
        public List<string> SalesAreaIdList { get; set; } = new List<string>();
        public string SalesAreaIds
        {
            get => string.Join(",", SalesAreaIdList.Select(s => s));
            set
            {
                SalesAreaIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<string> { value }
                        : new List<string> { };
            }
        }
        public List<string> TerritoryIdList { get; set; } = new List<string>();
        public string TerritoryIds
        {
            get => string.Join(",", TerritoryIdList.Select(s => s));
            set
            {
                TerritoryIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<string> { value }
                        : new List<string> { };
            }
        }
        public List<string> ZoneIdList { get; set; } = new List<string>();
        public string ZoneIds
        {
            get => string.Join(",", ZoneIdList.Select(s => s));
            set
            {
                ZoneIdList = !string.IsNullOrWhiteSpace(value) && value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : !string.IsNullOrWhiteSpace(value) && !value.Contains(",")
                        ? new List<string> { value }
                        : new List<string> { };
            }
        }

        public Dictionary<string, string> GetByName()
        {
            var result = new Dictionary<string, string>
            {
                {nameof(UserId), UserId.ToString() },
                {nameof(FullName), FullName??"" },
                {nameof(UserName), UserName??"" },
                {nameof(Phone), Phone??"" },
                {nameof(Email), Email??"" },
                {nameof(EmployeeId), EmployeeId??"" },
                {nameof(ActiveRoleId), ActiveRoleId.ToString() },
                {nameof(RoleIds), RoleIds??"" },
                {nameof(Avatar), Avatar??"" },
                {nameof(UserAgentInfo), UserAgentInfo??"" },
                {nameof(NodeId), NodeId.ToString() },
                {nameof(ActiveRoleName), ActiveRoleName??"" },
                {nameof(EmployeeRole), EmployeeRole.ToString() },
                {nameof(PlantIds), PlantIds??"" },
                {nameof(SalesOfficeIds), SalesOfficeIds??"" },
                {nameof(SalesAreaIds), SalesAreaIds??"" },
                {nameof(TerritoryIds), TerritoryIds??"" },
                {nameof(ZoneIds), ZoneIds??"" },
            };
            return result;
        }
    }
}
