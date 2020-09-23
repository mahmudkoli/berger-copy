using System.Collections.Generic;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Models.Organizations
{
    public class OrganizationRoleMappingModel
    {
        public OrganizationRoleMappingModel()
        {
            this.UserList = new List<int>();
        }
        public int Id { get; set; }

        public int RoleId { get; set; }
        public string UserName { get; set; }
        public OrganizationRoleModel Role { get; set; }

        public int UserInfoId { get; set; }
        public int UserId { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public List<int> UserList { get; set; }


    }
}
