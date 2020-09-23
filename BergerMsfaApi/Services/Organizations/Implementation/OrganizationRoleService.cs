using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Organizations;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Organizations.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Organizations.Implementation
{
    public class OrganizationRoleService : IOrganizationRoleService
    {
        private readonly IRepository<OrganizationRole> _organizationRole;
        private readonly IRepository<OrganizationUserRole> _organizationUserRole;
        private readonly IRepository<UserInfo> _userInfo;

        public OrganizationRoleService(IRepository<UserInfo>userInfo ,IRepository<OrganizationRole> organizationRole, IRepository<OrganizationUserRole> organizationUserRole)
        {
            _organizationRole = organizationRole;
            _organizationUserRole = organizationUserRole;
            _userInfo = userInfo;
        }

        // treeveiw Service Function

        public async Task<IEnumerable<OrganizationRoleModel>> GetOrganizationUserRoleAsync()
        {
            var result = await _organizationRole.GetAllAsync();

            var result2 = result.ToMap<OrganizationRole, OrganizationRoleModel>();
            var organizationRoleRawData = await _organizationUserRole.GetAllAsync();
             var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrganizationUserRole, OrganizationRoleMappingModel>()
                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.OrgRoleId));
            }).CreateMapper();

            var organizationUserRoleList =  mapper.Map<IEnumerable<OrganizationRoleMappingModel>>(organizationRoleRawData);
            var userInfoList = await _userInfo.GetAllAsync();


            foreach (var item in result2)
            {

                var tempRoleList = organizationUserRoleList.Where(c => c.RoleId == item.Id).ToList();
 
                foreach (var role in tempRoleList)
                {
                    var userInfo = userInfoList.Where(o => o.Id == role.UserId).FirstOrDefault();
                    role.UserName = userInfo.Name;
                }

                item.ConfigList = tempRoleList;


            }

            return result2;
        }



        public async Task<int> DeleteAsync(int id)
        {
            var result = await _organizationRole.DeleteAsync(s => s.Id == id);
            return result;

        }



        public async Task<bool> IsOrganizationRoleExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _organizationRole.IsExistAsync(s => s.Name == name)
                : await _organizationRole.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }
        

        public async Task<bool> IsOrganizationRoleLinkWithUserExistAsync(int roleid, int userinfoId)
        {
            var result = true;
            if (roleid > 0 && userinfoId > 0)
            {
                result = await _organizationUserRole.IsExistAsync(s => s.OrgRoleId == roleid && s.UserId == userinfoId);
            }
            return result;
        }

        public async Task<OrganizationRoleModel> GetOrganizationRoleAsync(int id)
        {
            var result = await _organizationRole.FindAsync(s => s.Id == id);
            return result.ToMap<OrganizationRole, OrganizationRoleModel>();
        }

        public async Task<List<int>>GetOrgenizationUserByRole(int orgRole)
        {
            var userlist =await _organizationUserRole.Where(u => u.OrgRoleId == orgRole).ToListAsync();
            return await userlist.Select(s => s.UserId).ToListAsync();
        }
        public async Task<IPagedList<OrganizationRoleModel>> GetPagedOrganizationRolesAsync(int pageNumber, int pageSize)
        {
            var result = await _organizationRole.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<OrganizationRole, OrganizationRoleModel>();

        }

        public async Task<OrganizationRoleModel> SaveAsync(OrganizationRoleModel model)
        {
            var organizationRole = model.ToMap<OrganizationRoleModel, OrganizationRole>();
            var result = await _organizationRole.CreateOrUpdateAsync(organizationRole);
            var usrList = model.UserList;
          
           // var CreateUserMappingList = new List<UserRoleMapping>();
            if (result.Id > 0 && usrList.Count > 0)
            {
                foreach (var item in usrList)
                {
                    await AddUserRoleMapping(item, result);
                }
               
            }


            return result.ToMap<OrganizationRole, OrganizationRoleModel>();
        }

        private async Task AddUserRoleMapping(int item, OrganizationRole result)
        {
          
            if (!await IsOrganizationRoleLinkWithUserExistAsync(result.Id, item).ConfigureAwait(false))
            {
                await _organizationUserRole.CreateOrUpdateAsync(new OrganizationUserRole
                {
                    UserId = item,
                    OrgRoleId = result.Id
                });
            }
        }

        public async Task<OrganizationRoleModel> UpdateAsync(OrganizationRoleModel model)
        {
            var organizationRole = model.ToMap<OrganizationRoleModel, OrganizationRole>();
            var result = await _organizationRole.UpdateAsync(organizationRole);
            await _organizationRole.ExecuteSqlCommandAsync($"Delete  from OrganizationUserRoles where OrgRoleid ={model.Id}");
            if (result.Id > 0 && model.UserList.Any())
            {
                foreach (var item in model.UserList)
                {
                    await AddUserRoleMapping(item, result);
                }

            }
            return result.ToMap<OrganizationRole, OrganizationRoleModel>();
        }

        public async Task<UserRoleMappingModel> SaveOrganizationRoleLinkWithUserAsync(UserRoleMappingModel model)
        {
            var example = new OrganizationUserRole {UserId = model.UserInfoId, OrgRoleId = model.RoleId};
            var result = await _organizationUserRole.CreateOrUpdateAsync(example);
            return result.ToMap<OrganizationUserRole, UserRoleMappingModel>();
        }

    }
}
