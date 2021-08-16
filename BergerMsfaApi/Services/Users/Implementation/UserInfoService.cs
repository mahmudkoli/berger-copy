using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRepository<UserInfo> _userInfoRepo;
        private readonly IRepository<UserZoneAreaMapping> _userZoneAreaRepo;
        private readonly IRepository<UserRoleMapping> _userRoleMappingRepo;
        private readonly IMapper _mapper;


        public UserInfoService(
            IRepository<UserInfo> userInfoRepo,
            IRepository<UserZoneAreaMapping> userZoneAreaRepo,
            IRepository<UserRoleMapping> userRoleMappingRepo,
            IMapper mapper)
        {
            _userInfoRepo = userInfoRepo;
            _userRoleMappingRepo = userRoleMappingRepo;
            _userZoneAreaRepo = userZoneAreaRepo;
            _mapper = mapper;
        }
       
        public async Task<UserInfoModel> CreateAsync(SaveUserInfoModel model)
        {
            var userInfo = model.ToMap<SaveUserInfoModel, UserInfo>();
            var result = await _userInfoRepo.CreateAsync(userInfo);
            var userZoneAreaMappingList = new List<UserZoneAreaMapping>();



            foreach (var p in model.PlantIds)
            {
                var u = new UserZoneAreaMapping();
                u.PlantId = p;
                u.UserInfoId = result.Id;

                if (model.SaleOfficeIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u); continue; };

                foreach (var s in model.SaleOfficeIds)
                {
                    var u1 = new UserZoneAreaMapping();
                    u1.SalesOfficeId = s;
                    u1.PlantId = u.PlantId;
                    u1.UserInfoId = result.Id;

                    if (model.AreaIds.Count == 0)
                    {
                        if (model.TerritoryIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u1); continue; };

                        foreach (var t in model.TerritoryIds)
                        {
                            var u3 = new UserZoneAreaMapping();
                            u3.SalesOfficeId = u1.SalesOfficeId;
                            u3.PlantId = u1.PlantId;
                            u3.UserInfoId = result.Id;
                            u3.AreaId = u1.AreaId;
                            u3.TerritoryId = t;

                            if (model.ZoneIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u3); continue; };

                            foreach (var z in model.ZoneIds)
                            {
                                var u4 = new UserZoneAreaMapping();
                                u4.SalesOfficeId = u3.SalesOfficeId;
                                u4.PlantId = u3.PlantId;
                                u4.UserInfoId = result.Id;
                                u4.AreaId = u3.AreaId;
                                u4.TerritoryId = u3.TerritoryId;
                                u4.ZoneId = z;

                                await _userZoneAreaRepo.CreateAsync(u4);
                            }
                        }
                    }
                    else
                    {
                        foreach (var a in model.AreaIds)
                        {
                            var u2 = new UserZoneAreaMapping();
                            u2.SalesOfficeId = u1.SalesOfficeId;
                            u2.PlantId = u1.PlantId;
                            u2.UserInfoId = result.Id;
                            u2.AreaId = a;

                            if (model.TerritoryIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u2); continue; };

                            foreach (var t in model.TerritoryIds)
                            {
                                var u3 = new UserZoneAreaMapping();
                                u3.SalesOfficeId = u2.SalesOfficeId;
                                u3.PlantId = u2.PlantId;
                                u3.UserInfoId = result.Id;
                                u3.AreaId = u2.AreaId;
                                u3.TerritoryId = t;

                                if (model.ZoneIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u3); continue; };

                                foreach (var z in model.ZoneIds)
                                {
                                    var u4 = new UserZoneAreaMapping();
                                    u4.SalesOfficeId = u3.SalesOfficeId;
                                    u4.PlantId = u3.PlantId;
                                    u4.UserInfoId = result.Id;
                                    u4.AreaId = u3.AreaId;
                                    u4.TerritoryId = u3.TerritoryId;
                                    u4.ZoneId = z;

                                    await _userZoneAreaRepo.CreateAsync(u4);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var role in model.RoleIds)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = role,
                    UserInfoId = result.Id
                };
                await CreateRoleLinkWithUserAsync(roleMappingModel);
            }

            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<UserRoleMappingModel> CreateRoleLinkWithUserAsync(UserRoleMappingModel model)
        {
            var userRoleMapping = model.ToMap<UserRoleMappingModel, UserRoleMapping>();
            var result = await _userRoleMappingRepo.CreateAsync(userRoleMapping);
            return result.ToMap<UserRoleMapping, UserRoleMappingModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var data = await GetUserRoleMappingByUserInfoIdAsync(id);

            if (data != null)
                await DeleteUserRoleMappingAsync(id);

            await _userZoneAreaRepo.DeleteAsync(f => f.UserInfoId == id);

            var result = await _userInfoRepo.DeleteAsync(s => s.Id == id);
           
            return result;
        }

        public async Task<int> DeleteUserRoleMappingAsync(int id)
        {
            var result = await _userRoleMappingRepo.DeleteAsync(s => s.UserInfoId == id);
            return result;
        }

        public async Task<bool> IsUserExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _userInfoRepo.IsExistAsync(s => s.FullName == name)
                : await _userInfoRepo.IsExistAsync(s => s.FullName == name && s.Id != id);

            return result;
        }

        public async Task<bool> IsUserNameExistAsync(string username, int id = 0)
        {
            return await _userInfoRepo.IsExistAsync(s => s.UserName.ToLower() == username.ToLower() && s.Id != id);
        }

        public async Task<bool> IsRoleLinkWithUserExistAsync(int roleid, int userinfoId)
        {
            var result = true;
            if (roleid > 0 && userinfoId > 0)
            {
                result = await _userRoleMappingRepo.IsExistAsync(s => s.RoleId == roleid && s.UserInfoId == userinfoId);
            }
            return result;
        }

        public async Task<bool> IsRoleMappingExistAsync(int userInfoId)
        {
            var result = await _userRoleMappingRepo.IsExistAsync(s => s.UserInfoId == userInfoId);
            return result;
        }

        public async Task<UserRoleMappingModel> GetUserRoleMappingByUserInfoIdAsync(int userinfoId)
        {
            var data = await _userRoleMappingRepo.FindAsync(u => u.UserInfoId == userinfoId);
            return data.ToMap<UserRoleMapping, UserRoleMappingModel>();
        }

        public async Task<UserInfoModel> GetUserAsync(int id)
        {
            var  result = await _userInfoRepo.FindAsync(s => s.Id == id);
            var areaZone =  await _userZoneAreaRepo.FindAllAsync(f => f.UserInfoId == id);
            var plantIds = await (from p in areaZone select p.PlantId).Distinct().ToListAsync();
            var saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            var areaIds = await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            var territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            var zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            var roleIds = await (from s in _userRoleMappingRepo.GetAll() where s.UserInfoId == id select s.RoleId).Distinct().ToListAsync();

            var finalResult = result.ToMap<UserInfo, UserInfoModel>();
           
            finalResult.PlantIds = plantIds;
            finalResult.SaleOfficeIds = saleOfficeIds;
            finalResult.AreaIds = areaIds;
            finalResult.TerritoryIds = territoryIds;
            finalResult.ZoneIds = zoneIds;
            finalResult.RoleIds = roleIds;

            //var userRoleMapping = await _userRoleMappingRepo.FindAsync(u => u.UserInfoId == id);
            var userRoleMapping = await _userRoleMappingRepo.FindAllInclude(u => u.UserInfoId == result.Id, i => i.Role).FirstOrDefaultAsync();

            if (userRoleMapping != null)
            {
                finalResult.RoleId = userRoleMapping.RoleId;
                finalResult.RoleName = userRoleMapping.Role?.Name;
            }

            return finalResult;
        }

        public async Task<UserInfoModel> GetUserByUserNameAsync(string username)
        {
            var  result = await _userInfoRepo.FindAsync(s => s.UserName.ToLower() == username.ToLower());
            var areaZone =  await _userZoneAreaRepo.FindAllAsync(f => f.UserInfoId == result.Id);
            var plantIds = await (from p in areaZone select p.PlantId).Distinct().ToListAsync();
            var saleOfficeIds = await (from s in areaZone where s.SalesOfficeId != null select s.SalesOfficeId).Distinct().ToListAsync();
            var areaIds = await (from s in areaZone where s.AreaId != null select s.AreaId).Distinct().ToListAsync(); ;
            var territoryIds = await (from s in areaZone where s.TerritoryId != null select s.TerritoryId).Distinct().ToListAsync(); ;
            var zoneIds = await (from s in areaZone where s.ZoneId != null select s.ZoneId).Distinct().ToListAsync();
            var roleIds = await (from s in _userRoleMappingRepo.GetAll() where s.UserInfoId == result.Id select s.RoleId).Distinct().ToListAsync();

            var finalResult = result.ToMap<UserInfo, UserInfoModel>();
           
            finalResult.PlantIds = plantIds;
            finalResult.SaleOfficeIds = saleOfficeIds;
            finalResult.AreaIds = areaIds;
            finalResult.TerritoryIds = territoryIds;
            finalResult.ZoneIds = zoneIds;
            finalResult.RoleIds = roleIds;

            var userRoleMapping = await _userRoleMappingRepo.FindAllInclude(u => u.UserInfoId == result.Id, i=> i.Role).FirstOrDefaultAsync();

            if (userRoleMapping != null)
            {
                finalResult.RoleId = userRoleMapping.RoleId;
                finalResult.RoleName = userRoleMapping.Role?.Name;
            }

            return finalResult;
        }

        public async Task<QueryResultModel<UserInfoModel>> GetUsersAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<UserInfo, object>>>()
            {
                ["userName"] = v => v.UserName,
                ["fullName"] = v => v.FullName,
                ["designation"] = v => v.Designation,
                ["department"] = v => v.Department,
            };

            var result = await _userInfoRepo.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.FullName.Contains(query.GlobalSearchValue) || x.Department.Contains(query.GlobalSearchValue) || x.Designation.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.Roles).ThenInclude(i => i.Role),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<UserInfoModel>>(result.Items);

            var queryResult = new QueryResultModel<UserInfoModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            var result = await _userInfoRepo.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync()
        {
            var result = await _userInfoRepo.ExecuteQueryAsyc<UserInfoModel>("SELECT * FROM Users");
            return result;
        }

        public async Task<UserInfoModel> SaveAsync(UserInfoModel model)
        {
            var userInfo = model.ToMap<UserInfoModel, UserInfo>();
            var result = await _userInfoRepo.CreateOrUpdateAsync(userInfo);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<UserRoleMappingModel> SaveRoleLinkWithUserAsync(UserRoleMappingModel model)
        {
            var isExist = await IsRoleMappingExistAsync(model.UserInfoId);
            if (isExist)
                await DeleteUserRoleMappingAsync(model.UserInfoId);

            var userRoleMapping = model.ToMap<UserRoleMappingModel, UserRoleMapping>();
            var result = await _userRoleMappingRepo.CreateOrUpdateAsync(userRoleMapping);
            return result.ToMap<UserRoleMapping, UserRoleMappingModel>();
        }

        public async Task<UserInfoModel> UpdateAsync(SaveUserInfoModel model)
        {
            var userInfo = model.ToMap<SaveUserInfoModel, UserInfo>();
            var result = await _userInfoRepo.UpdateAsync(userInfo);

            var any = await  _userZoneAreaRepo.AnyAsync(f => f.UserInfoId == result.Id);
            if (any)
               await _userZoneAreaRepo.DeleteAsync(f => f.UserInfoId == model.Id);

            foreach (var p in model.PlantIds)
            {
                var u = new UserZoneAreaMapping();
                u.PlantId = p;
                u.UserInfoId = result.Id;

                if (model.SaleOfficeIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u); continue; };

                foreach (var s in model.SaleOfficeIds)
                {
                    var u1 = new UserZoneAreaMapping();
                    u1.SalesOfficeId = s;
                    u1.PlantId = u.PlantId;
                    u1.UserInfoId = result.Id;

                    if (model.AreaIds.Count == 0) 
                    {
                        if (model.TerritoryIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u1); continue; };

                        foreach (var t in model.TerritoryIds)
                        {
                            var u3 = new UserZoneAreaMapping();
                            u3.SalesOfficeId = u1.SalesOfficeId;
                            u3.PlantId = u1.PlantId;
                            u3.UserInfoId = result.Id;
                            u3.AreaId = u1.AreaId;
                            u3.TerritoryId = t;

                            if (model.ZoneIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u3); continue; };

                            foreach (var z in model.ZoneIds)
                            {
                                var u4 = new UserZoneAreaMapping();
                                u4.SalesOfficeId = u3.SalesOfficeId;
                                u4.PlantId = u3.PlantId;
                                u4.UserInfoId = result.Id;
                                u4.AreaId = u3.AreaId;
                                u4.TerritoryId = u3.TerritoryId;
                                u4.ZoneId = z;

                                await _userZoneAreaRepo.CreateAsync(u4);
                            }
                        }
                    }
                    else
                    {
                        foreach (var a in model.AreaIds)
                        {
                            var u2 = new UserZoneAreaMapping();
                            u2.SalesOfficeId = u1.SalesOfficeId;
                            u2.PlantId = u1.PlantId;
                            u2.UserInfoId = result.Id;
                            u2.AreaId = a;

                            if (model.TerritoryIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u2); continue; };

                            foreach (var t in model.TerritoryIds)
                            {
                                var u3 = new UserZoneAreaMapping();
                                u3.SalesOfficeId = u2.SalesOfficeId;
                                u3.PlantId = u2.PlantId;
                                u3.UserInfoId = result.Id;
                                u3.AreaId = u2.AreaId;
                                u3.TerritoryId = t;

                                if (model.ZoneIds.Count == 0) { await _userZoneAreaRepo.CreateAsync(u3); continue; };

                                foreach (var z in model.ZoneIds)
                                {
                                    var u4 = new UserZoneAreaMapping();
                                    u4.SalesOfficeId = u3.SalesOfficeId;
                                    u4.PlantId = u3.PlantId;
                                    u4.UserInfoId = result.Id;
                                    u4.AreaId = u3.AreaId;
                                    u4.TerritoryId = u3.TerritoryId;
                                    u4.ZoneId = z;

                                    await _userZoneAreaRepo.CreateAsync(u4);
                                }
                            }
                        }
                    }
                }
            }


            any = await _userRoleMappingRepo.AnyAsync(f => f.UserInfoId == result.Id);
            if (any)
                await _userRoleMappingRepo.DeleteAsync(f => f.UserInfoId == model.Id);

            foreach (var role in model.RoleIds)
            {
                UserRoleMappingModel roleMappingModel = new UserRoleMappingModel
                {
                    RoleId = role,
                    UserInfoId = result.Id
                };

                //var userRoleMappingData = await GetUserRoleMappingByUserInfoIdAsync(result.Id);
                //if (userRoleMappingData != null)
                //{
                //    if (userRoleMappingData.RoleId != model.RoleId)
                //    {
                //        await DeleteUserRoleMappingAsync(result.Id);
                //        await CreateRoleLinkWithUserAsync(roleMappingModel);
                //    }
                //}
                //else
                //{
                    await CreateRoleLinkWithUserAsync(roleMappingModel);
                //}
            }

            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<bool> IsUserExistAsync(string adguid)
        {
            var result = await _userInfoRepo.AnyAsync(s => s.PhoneNumber.Trim() == adguid.Trim());
            return result;
        }

        //public async Task<IEnumerable<UserInfoModel>> GetFMUsersAsync(int id)
        //{
        //    var result = await _user.GetAllAsync();
        //    return result.ToMap<UserInfo, UserInfoModel>();
        //}

        public async Task<int?> GetFMUserIdByNameAsync(string name)
        {
            return (await _userInfoRepo.FindAsync(x => x.FullName.ToUpper() == name.ToUpper()))?.Id;
        }

        public async Task<int?> GetFMUserIdByEmailAsync(string email)
        {
            return (await _userInfoRepo.FindAsync(x => x.Email.ToUpper() == email.ToUpper()))?.Id;
        }
    }
}
