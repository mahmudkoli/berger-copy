using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class OrganizationUserService : IOrganizationUserService
    {
        private readonly IRepository<UserInfo> _user;
        private readonly IRepository<OrganizationUserRole> _organizationRoleMapping;

        public OrganizationUserService(IRepository<UserInfo> user, IRepository<OrganizationUserRole> organizationRoleMapping)
        {
            _user = user;
            _organizationRoleMapping = organizationRoleMapping;
        }

        public async Task<IEnumerable<UserInfoModel>> GetUsersAsync()
        {
            var result = await _user.GetAllAsync();
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            var result = await _user.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync()
        {
            var result = await _user.ExecuteQueryAsyc<UserInfoModel>("SELECT * FROM UserInfos");
            return result;
        }
        public async Task<UserInfoModel> GetUserAsync(int id)
        {
            var result = await _user.FindAsync(s => s.Id == id);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<UserInfoModel> SaveAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.CreateOrUpdateAsync(example);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<UserInfoModel> CreateAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.CreateAsync(example);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<OrganizationRoleMappingModel> SaveOrganizationRoleLinkWithUserAsync(OrganizationRoleMappingModel model)
        {
            var example = model.ToMap<OrganizationRoleMappingModel, OrganizationUserRole>();
            var result = await _organizationRoleMapping.CreateOrUpdateAsync(example);
            return result.ToMap<OrganizationUserRole, OrganizationRoleMappingModel>();
        }

        public async Task<OrganizationRoleMappingModel> CreateOrganizationRoleLinkWithUserAsync(OrganizationRoleMappingModel model)
        {
            var example = model.ToMap<OrganizationRoleMappingModel, OrganizationUserRole>();
            var result = await _organizationRoleMapping.CreateAsync(example);
            return result.ToMap<OrganizationUserRole, OrganizationRoleMappingModel>();
        }

        public async Task<UserInfoModel> UpdateAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.UpdateAsync(example);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _user.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsUserExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.Name == name)
                : await _user.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }

        public async Task<bool> IsOrganizationRoleLinkWithUserExistAsync(int organizationRoleId, int userinfoId)
        {
            var result = true;
            if (organizationRoleId > 0 && userinfoId > 0)
            {
                result = await _organizationRoleMapping.IsExistAsync(s => s.OrgRoleId == organizationRoleId && s.UserId == userinfoId);
            }
            return result;
        }
        

        

        

        

        

        

        


    }
}
