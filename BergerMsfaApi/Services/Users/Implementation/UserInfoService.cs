using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
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
        private readonly IRepository<UserZoneAreaMapping> _userZoneArea;

        private readonly IRepository<UserRoleMapping> _userRoleMapping;
        private readonly IRepository<UserTerritoryMapping> _userTerritoryMapping;


        public UserInfoService(IRepository<UserInfo> role,
            IRepository<UserZoneAreaMapping> userZoneArea,
        IRepository<UserRoleMapping> userRoleMapping, IRepository<UserTerritoryMapping> userTerritoryMapping)
        {
            _user = role;
            _userRoleMapping = userRoleMapping;
            _userTerritoryMapping = userTerritoryMapping;
            _userZoneArea = userZoneArea;
        }

       
        public async Task<UserInfoModel> CreateAsync(UserInfoModel model)
        {
            var example = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _user.CreateAsync(example);
            var userZoneAreaMappingList = new List<UserZoneAreaMapping>();
            // var u = new UserZoneAreaMapping();

            //foreach (var v in model.plantIds)
            //  await  _userZoneArea.DeleteAsync(f => f.PlanId == v && f.UserInfoId == result.Id);

            foreach (var p in model.plantIds)
            {
                var u= new UserZoneAreaMapping();
                u.PlanId = p;
                u.UserInfoId = result.Id;
                if (model.saleOfficeIds.Count == 0) { await _userZoneArea.CreateAsync(u); continue; };
        
                foreach (var s in model.saleOfficeIds)
                {
                    var u1 = new UserZoneAreaMapping();
                    u1.SalesOfficeId = s;
                    u1.PlanId = u.PlanId;
                    u1.UserInfoId = result.Id;
                    if (model.areaIds.Count == 0) { await _userZoneArea.CreateAsync(u1); continue; };
                    foreach (var a in model.areaIds)
                    {
                        var u2 = new UserZoneAreaMapping();
                        u2.SalesOfficeId = u1.SalesOfficeId;
                        u2.PlanId = u1.PlanId;
                        u2.UserInfoId = result.Id;
                        u2.AreaId = a;
                        if (model.territoryIds.Count == 0) { await _userZoneArea.CreateAsync(u2); continue; };
                        foreach (var t in model.territoryIds)
                        {
                            var u3 = new UserZoneAreaMapping();
                            u3.SalesOfficeId = u2.SalesOfficeId;
                            u3.PlanId = u2.PlanId;
                            u3.UserInfoId = result.Id;
                            u3.AreaId = u2.AreaId;
                            u3.TerritoryId = t;
                            if (model.zoneIds.Count == 0) { await _userZoneArea.CreateAsync(u3); continue; };
                            foreach (var z in model.zoneIds)
                            {
                                var u4 = new UserZoneAreaMapping();
                                u4.SalesOfficeId = u3.SalesOfficeId;
                                u4.PlanId = u3.PlanId;
                                u4.UserInfoId = result.Id;
                                u4.AreaId = u3.AreaId;
                                u4.TerritoryId =u3.TerritoryId;
                                u4.ZoneId = z;
                                await _userZoneArea.CreateAsync(u4);
                                //u = new UserZoneAreaMapping();
                            }
                        }
                    }
                }

            }
          

            //var nodeIdList = new List<int>();

            //if (model.NationalNodeIds.Count > 0)
            //    nodeIdList = model.NationalNodeIds;
            //else if (model.RegionNodeIds.Count > 0)
            //    nodeIdList = model.RegionNodeIds;
            //else if (model.AreaNodeIds.Count > 0)
            //    nodeIdList = model.AreaNodeIds;
            //else if (model.TerritoryNodeIds.Count > 0)
            //    nodeIdList = model.TerritoryNodeIds;


            //var CreateUserTerritoryMappingList = new List<UserTerritoryMapping>();


            //if (result.Id > 0 && nodeIdList.Count > 0)
            //{

            //    foreach (var item in nodeIdList)
            //    {

            //        UserTerritoryMapping mappingModel = new UserTerritoryMapping
            //        {
            //            UserInfoId = result.Id,
            //            NodeId = item
            //        };

            //        CreateUserTerritoryMappingList.Add(mappingModel);

            //    }

            //    var CreateFeedbackdata = await _userTerritoryMapping.CreateListAsync(CreateUserTerritoryMappingList);

            //}
            foreach (var role in model.RoleIds)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = role,
                    UserInfoId = result.Id
                };
                await CreateRoleLinkWithUserAsync(roleMappingModel);
            }

            //if (model.RoleId > 0)
            //{
            //    UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
            //    {
            //        RoleId = model.RoleId,
            //        UserInfoId = result.Id
            //    };
            //    await CreateRoleLinkWithUserAsync(roleMappingModel);
            //}

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

            await _userZoneArea.DeleteAsync(f => f.UserInfoId == id);

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

        public async Task<bool> IsUserNameExistAsync(string username, int id = 0)
        {
            return await _user.IsExistAsync(s => s.UserName.Equals(username) && s.Id != id);
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
            
            var  result = await _user.FindAsync(s => s.Id == id);
            var areaZone =  await _userZoneArea.FindAllAsync(f => f.UserInfoId == id);
            var plantIds = await (from p in areaZone select p.PlanId).Distinct().ToListAsync();
            var saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            var areaIds = await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            var territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            var zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            var finalResult = result.ToMap<UserInfo, UserInfoModel>();
           
            finalResult.plantIds = plantIds;
            finalResult.saleOfficeIds = saleOfficeIds;
            finalResult.areaIds = areaIds;
            finalResult.territoryIds = territoryIds;
            finalResult.zoneIds = zoneIds;

            //finalResult.plantIds = await (from p in areaZone select p.PlanId).Distinct().ToListAsync();
            //finalResult.saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            //finalResult.areaIds =  await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            //finalResult.territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            //finalResult.zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            //finalResult.plantIds = (from p in areaZone
            //                        select p.PlanId).ToList();
            //finalResult.plantIds = areaZone.Select(s => s.PlanId).Distinct().ToList();
            //finalResult.saleOfficeIds = areaZone.Select(c => c.SalesOfficeId).ToList();
            //finalResult.areaIds = areaZone.Select(c => c.AreaId).ToList();
            //finalResult.territoryIds = areaZone.Select(c => c.TerritoryId).ToList();
            //finalResult.zoneIds = areaZone.Select(c => c.ZoneId).Distinct().ToList();
            // finalResult.RoleIds = result.UserZoneAreaMappings.Select(c => c.R).Distinct().ToList();


            var userRoleMapping = await _userRoleMapping.FindAsync(u => u.UserInfoId == id);

            //if (userRoleMapping != null)  
            //{
            //    finalResult.RoleId = userRoleMapping.RoleId;
            //}
            return finalResult;
        }

        public async Task<UserInfoModel> GetUserByUserNameAsync(string username)
        {
            
            var  result = await _user.FindAsync(s => s.UserName.Equals(username));
            var areaZone =  await _userZoneArea.FindAllAsync(f => f.UserInfoId == result.Id);
            var plantIds = await (from p in areaZone select p.PlanId).Distinct().ToListAsync();
            var saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            var areaIds = await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            var territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            var zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            var finalResult = result.ToMap<UserInfo, UserInfoModel>();
           
            finalResult.plantIds = plantIds;
            finalResult.saleOfficeIds = saleOfficeIds;
            finalResult.areaIds = areaIds;
            finalResult.territoryIds = territoryIds;
            finalResult.zoneIds = zoneIds;

            //finalResult.plantIds = await (from p in areaZone select p.PlanId).Distinct().ToListAsync();
            //finalResult.saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            //finalResult.areaIds =  await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            //finalResult.territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            //finalResult.zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            //finalResult.plantIds = (from p in areaZone
            //                        select p.PlanId).ToList();
            //finalResult.plantIds = areaZone.Select(s => s.PlanId).Distinct().ToList();
            //finalResult.saleOfficeIds = areaZone.Select(c => c.SalesOfficeId).ToList();
            //finalResult.areaIds = areaZone.Select(c => c.AreaId).ToList();
            //finalResult.territoryIds = areaZone.Select(c => c.TerritoryId).ToList();
            //finalResult.zoneIds = areaZone.Select(c => c.ZoneId).Distinct().ToList();
            // finalResult.RoleIds = result.UserZoneAreaMappings.Select(c => c.R).Distinct().ToList();


            var userRoleMapping = await _userRoleMapping.FindAsync(u => u.UserInfoId == result.Id);

            //if (userRoleMapping != null)  
            //{
            //    finalResult.RoleId = userRoleMapping.RoleId;
            //}
            return finalResult;
        }


        public async Task<IEnumerable<UserInfoModel>> GetUsersAsync()
        {
            var result = await _user.GetAllAsync();
            //var appUserId = AppIdentity.AppUser.UserId;
            ////var appUserId = 54;
            //var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            //var result = new List<UserInfo>();
            //if (isAdmin)
            //{
            //    result = _user.GetAll().ToList();
            //}
            //else
            //{
            //    result = _user.GetNodeWiseUsersByUserId(appUserId).ToList();
            //}


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
            if (isExist)
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
             var any=await  _userZoneArea.AnyAsync(f => f.UserInfoId == result.Id);
            if (any)
               await _userZoneArea.DeleteAsync(f => f.UserInfoId == model.Id);

            foreach (var p in model.plantIds)
            {
                var u = new UserZoneAreaMapping();
                u.PlanId = p;
                u.UserInfoId = result.Id;
                if (model.saleOfficeIds.Count == 0) { await _userZoneArea.CreateAsync(u); continue; };

                foreach (var s in model.saleOfficeIds)
                {
                    var u1 = new UserZoneAreaMapping();
                    u1.SalesOfficeId = s;
                    u1.PlanId = u.PlanId;
                    u1.UserInfoId = result.Id;
                    if (model.areaIds.Count == 0) { await _userZoneArea.CreateAsync(u1); continue; };
                    foreach (var a in model.areaIds)
                    {
                        var u2 = new UserZoneAreaMapping();
                        u2.SalesOfficeId = u1.SalesOfficeId;
                        u2.PlanId = u1.PlanId;
                        u2.UserInfoId = result.Id;
                        u2.AreaId = a;
                        if (model.territoryIds.Count == 0) { await _userZoneArea.CreateAsync(u2); continue; };
                        foreach (var t in model.territoryIds)
                        {
                            var u3 = new UserZoneAreaMapping();
                            u3.SalesOfficeId = u2.SalesOfficeId;
                            u3.PlanId = u2.PlanId;
                            u3.UserInfoId = result.Id;
                            u3.AreaId = u2.AreaId;
                            u3.TerritoryId = t;
                            if (model.zoneIds.Count == 0) { await _userZoneArea.CreateAsync(u3); continue; };
                            foreach (var z in model.zoneIds)
                            {
                                var u4 = new UserZoneAreaMapping();
                                u4.SalesOfficeId = u3.SalesOfficeId;
                                u4.PlanId = u3.PlanId;
                                u4.UserInfoId = result.Id;
                                u4.AreaId = u3.AreaId;
                                u4.TerritoryId = u3.TerritoryId;
                                u4.ZoneId = z;
                                await _userZoneArea.CreateAsync(u4);
                                //u = new UserZoneAreaMapping();
                            }
                        }
                    }
                }

            }
            //var nodeIdList = new List<int>();

            //if (model.NationalNodeIds.Count > 0)
            //    nodeIdList = model.NationalNodeIds;
            //else if (model.RegionNodeIds.Count > 0)
            //    nodeIdList = model.RegionNodeIds;
            //else if (model.AreaNodeIds.Count > 0)
            //    nodeIdList = model.AreaNodeIds;
            //else if (model.TerritoryNodeIds.Count > 0)
            //    nodeIdList = model.TerritoryNodeIds;

            foreach (var role in model.RoleIds)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = role,
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


            //if (model.RoleId > 0)
            //{
            //    UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
            //    {
            //        RoleId = model.RoleId,
            //        UserInfoId = result.Id
            //    };

            //    var userRoleMappingData = await GetUserRoleMappingByUserInfoIdAsync(result.Id);
            //    if (userRoleMappingData != null)
            //    {
            //        if (userRoleMappingData.RoleId != model.RoleId)
            //        {
            //            var delFeedback = await DeleteUserRoleMappingAsync(result.Id);
            //            await CreateRoleLinkWithUserAsync(roleMappingModel);
            //        }
            //    }
            //    else
            //    {
            //        await CreateRoleLinkWithUserAsync(roleMappingModel);
            //    }

            //}

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
            return (await _user.FindAsync(x => x.Email.ToUpper() == email.ToUpper()))?.Id;
        }


    }
}
