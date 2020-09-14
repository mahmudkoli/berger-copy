using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Domain.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class UserInfoService : IUserInfoService
    {
        //private readonly IRepository<UserInfo> _user;
        //public UserInfoService(IRepository<UserInfo> user)
        //{
        //    _user = user;
        //}
        //public async Task<UserInfoViewModel> CreateUserAsync(UserInfoViewModel model)
        //{
        //    var user = model.ToMap<UserInfoViewModel, UserInfo>();

        //    var result = await _user.CreateAsync(user);
        //    return result.ToMap<UserInfo, UserInfoViewModel>();
        //}

        //public async Task<int> DeleteAsync(int id)
        //{
        //    var result = await _user.DeleteAsync(s => s.Id == id);
        //    return result;
        //}

        //public async Task<IEnumerable<UserInfoViewModel>> GetAllUserAsync()
        //{
        //    var result = await _user.GetAllAsync();
        //    return result.ToMap<UserInfo, UserInfoViewModel>();
        //}

        //public async Task<UserInfoViewModel> GetUserAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<UserInfoViewModel> UpdateAsync(UserInfoViewModel model)
        //{
        //    throw new NotImplementedException();
        //}







        private readonly IRepository<UserInfo> _user;

        private readonly IRepository<UserRoleMapping> _userRoleMapping;
        private readonly IRepository<UserTerritoryMapping> _userTerritoryMapping;


        public UserInfoService(IRepository<UserInfo> role, IRepository<UserRoleMapping> userRoleMapping, IRepository<UserTerritoryMapping> userTerritoryMapping)
        {
            _user = role;
            _userRoleMapping = userRoleMapping;
            _userTerritoryMapping = userTerritoryMapping;
          
        }


        public async Task<UserInfoModel> CreateAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.CreateAsync(example);

            var nodeIdList = new List<int>();

            if (model.NationalNodeIds.Count > 0)
                nodeIdList = model.NationalNodeIds;
            else if (model.RegionNodeIds.Count > 0)
                nodeIdList = model.RegionNodeIds;
            else if (model.AreaNodeIds.Count > 0)
                nodeIdList = model.AreaNodeIds;
            else if (model.TerritoryNodeIds.Count > 0)
                nodeIdList = model.TerritoryNodeIds;


            var CreateUserTerritoryMappingList = new List<UserTerritoryMapping>();


            if (result.Id > 0 && nodeIdList.Count > 0)
            {

                foreach (var item in nodeIdList)
                {

                    UserTerritoryMapping mappingModel = new UserTerritoryMapping
                    {
                        UserInfoId = result.Id,
                        NodeId = item
                    };

                    CreateUserTerritoryMappingList.Add(mappingModel);

                }

                var CreateFeedbackdata = await _userTerritoryMapping.CreateListAsync(CreateUserTerritoryMappingList);

            }
            if (model.RoleId > 0)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = model.RoleId,
                    UserInfoId = result.Id
                };
                await CreateRoleLinkWithUserAsync(roleMappingModel);
            }

            return result.ToMap<UserInfo, UserInfoModel>();
        }
        public async Task<UserRoleMappingModel> CreateRoleLinkWithUserAsync(UserRoleMappingModel model)
        {
            var example = model.ToMap<UserRoleMappingModel, UserRoleMapping>();
            var result = await _userRoleMapping.CreateAsync(example);
            return result.ToMap<UserRoleMapping, UserRoleMappingModel>();
        }

    

        public async Task<int> DeleteAsync(int id)
        {

            var data1 = await GetUserRoleMappingByUserInfoIdAsync(id);

            if (data1 != null)
            {
                var delFeedback = await DeleteUserRoleMappingAsync(id);
            }

         



            var result = await _user.DeleteAsync(s => s.Id == id);
            return result;
        }

        public async Task<int> DeleteUserRoleMappingAsync(int id)
        {
            var result = await _userRoleMapping.DeleteAsync(s => s.UserInfoId == id);
            return result;
        }

        public async Task<int> DeleteUserTerritoryMappingAsync(int id)
        {
            var result = await _userTerritoryMapping.DeleteAsync(s => s.UserInfoId == id);
            return result;
        }




        public async Task<bool> IsUserExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.Name == name)
                : await _user.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }
        public async Task<bool> IsRoleLinkWithUserExistAsync(int roleid, int userinfoId)
        {
            var result = true;
            if (roleid > 0 && userinfoId > 0)
            {
                result = await _userRoleMapping.IsExistAsync(s => s.RoleId == roleid && s.UserInfoId == userinfoId);
            }
            return result;
        }
        public async Task<bool> IsTerritoryLinkWithUserExistAsync(int nodeid, int userinfoId)
        {
            var result = true;
            if (nodeid > 0 && userinfoId > 0)
            {
                result = await _userTerritoryMapping.IsExistAsync(s => s.NodeId == nodeid && s.UserInfoId == userinfoId);
            }
            return result;
        }
        public async Task<bool> IsRoleMappingExistAsync(int userInfoId)
        {
            var result = await _userRoleMapping.IsExistAsync(s => s.UserInfoId == userInfoId);
            return result;
        }

  

        public async Task<UserRoleMappingModel> GetUserRoleMappingByUserInfoIdAsync(int userinfoId)
        {

            var data = await _userRoleMapping.FindAsync(u => u.UserInfoId == userinfoId);
            return data.ToMap<UserRoleMapping, UserRoleMappingModel>();

        }


        public async Task<UserInfoModel> GetUserAsync(int id)
        {
            var result = await _user.FindAsync(s => s.Id == id);
            var finalResult = result.ToMap<UserInfo, UserInfoModel>();


            var userRoleMapping = await _userRoleMapping.FindAsync(u => u.UserInfoId == id);

            if (userRoleMapping != null)
            {
                finalResult.RoleId = userRoleMapping.RoleId;
            }
            return finalResult;
        }


        public async Task<IEnumerable<UserInfoModel>> GetUsersAsync()
        {
            var appUserId = AppIdentity.AppUser.UserId;
            //var appUserId = 54;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            var result = new List<UserInfo>();
            if (isAdmin)
            {
                result = _user.GetAll().ToList();
            }
            else
            {
                result = _user.GetNodeWiseUsersByUserId(appUserId).ToList();
            }


            return result.ToMap<UserInfo, UserInfoModel>();
        }


        public async Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            var result = await _user.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<UserInfo, UserInfoModel>();

        }

        public async Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync()
        {
            var result = await _user.ExecuteQueryAsyc<UserInfoModel>("SELECT * FROM Users");
            return result;
        }

        public async Task<UserInfoModel> SaveAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.CreateOrUpdateAsync(example);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<UserRoleMappingModel> SaveRoleLinkWithUserAsync(UserRoleMappingModel model)
        {
            var isExist = await IsRoleMappingExistAsync(model.UserInfoId);
            if(isExist)
            {
                var delFeedback = await DeleteUserRoleMappingAsync(model.UserInfoId);
            }

            var example = model.ToMap<UserRoleMappingModel, UserRoleMapping>();
            var result = await _userRoleMapping.CreateOrUpdateAsync(example);
            return result.ToMap<UserRoleMapping, UserRoleMappingModel>();
        }
      

        public async Task<UserInfoModel> UpdateAsync(UserInfoModel model)
        {

            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.UpdateAsync(example);

            var nodeIdList = new List<int>();

            if (model.NationalNodeIds.Count > 0)
                nodeIdList = model.NationalNodeIds;
            else if (model.RegionNodeIds.Count > 0)
                nodeIdList = model.RegionNodeIds;
            else if (model.AreaNodeIds.Count > 0)
                nodeIdList = model.AreaNodeIds;
            else if (model.TerritoryNodeIds.Count > 0)
                nodeIdList = model.TerritoryNodeIds;

       


            if (model.RoleId > 0)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = model.RoleId,
                    UserInfoId = result.Id
                };

                var userRoleMappingData = await GetUserRoleMappingByUserInfoIdAsync(result.Id);
                if (userRoleMappingData != null)
                {
                    if (userRoleMappingData.RoleId != model.RoleId)
                    {
                        var delFeedback = await DeleteUserRoleMappingAsync(result.Id);
                        await CreateRoleLinkWithUserAsync(roleMappingModel);
                    }
                }
                else
                {
                    await CreateRoleLinkWithUserAsync(roleMappingModel);
                }

            }

            return result.ToMap<UserInfo, UserInfoModel>();
        }
        public async Task<bool> IsUserExistAsync(string adguid)
        {
            var result = await _user.AnyAsync(s => s.PhoneNumber.Trim() == adguid.Trim());

            return result;
        }
        public async Task<UserInfoModel> GetCurrentUser(string adguid)
        {
            var result = await _user.FirstOrDefaultAsync(s => s.AdGuid.Trim() == adguid.Trim());
            var roles = _userRoleMapping.GetAllInclude(x => x.Role);
            var userTerr = await _userTerritoryMapping.FirstOrDefaultAsync(a => a.UserInfoId == result.Id);

            //model mapping
            var finalResult = result.ToMap<UserInfo, UserInfoModel>();
            finalResult.RoleIds = roles.Where(a => a.UserInfoId == result.Id).Select(a => a.RoleId).ToList();
            if (finalResult.RoleIds.Count > 0)
                finalResult.RoleId = finalResult.RoleIds[0];
            if (userTerr != null)
                finalResult.NodeId = userTerr.NodeId;

            finalResult.RoleName = roles.Where(a => a.UserInfoId == result.Id).Select(a => a.Role.Name).FirstOrDefault();

            return finalResult;
        }

        //public async Task<IEnumerable<UserInfoModel>> GetFMUsersAsync(int id)
        //{
        //    var result = await _user.GetAllAsync();
        //    return result.ToMap<UserInfo, UserInfoModel>();
        //}

        public async Task<int?> GetFMUserIdByNameAsync(string name)
        {
            return (await _user.FindAsync(x => x.Name.ToUpper() == name.ToUpper()))?.Id;
        }

        public async Task<int?> GetFMUserIdByEmailAsync(string email)
        {
            return (await _user.FindAsync(x => x.email.ToUpper() == email.ToUpper()))?.Id;
        }


    }
}
