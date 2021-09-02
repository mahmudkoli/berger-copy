using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Common.Model;
using BergerMsfaApi.Core;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Menus.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BergerMsfaApi.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly TokensSettingsModel _settings;
        private readonly IUserInfoService _userService;
        private readonly IMenuService _menuService;
        private readonly ICommonService _commonService;

        public AuthService(
            IOptions<TokensSettingsModel> settings,
            IUserInfoService user,
            ICommonService commonService, IMenuService menuService)
        {
            _settings = settings.Value;
            _userService = user;
            _commonService = commonService;
            _menuService = menuService;
        }

        public async Task<AuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName)
        {
            try
            {
                var userInfo = await _userService.GetUserByUserNameAsync(userName);
                var empMenu =await _menuService.GetPermissionMenusByEmpRoleId((int)userInfo.EmployeeRole);

                var userPrincipal = new AppUserPrincipal(userInfo.UserName)
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    ActiveRoleId = userInfo.RoleId,
                    RoleIdList = userInfo.RoleIds,
                    Avatar = "/img/user.png",
                    FullName = userInfo.FullName,
                    EmployeeId = userInfo.EmployeeId,
                    Phone = userInfo.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",
                    //NodeId = userInfo.NodeId,
                    ActiveRoleName = userInfo.RoleName,
                    EmployeeRole = (int)userInfo.EmployeeRole,
                    PlantIdList = userInfo.PlantIds,
                    SalesOfficeIdList = userInfo.SaleOfficeIds,
                    SalesAreaIdList = userInfo.AreaIds,
                    TerritoryIdList = userInfo.TerritoryIds,
                    ZoneIdList = userInfo.ZoneIds,
                };

                var appClaimes = userPrincipal.GetByName().Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName,userPrincipal.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub,userPrincipal.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                claims.AddRange(appClaimes);
                foreach (var role in userPrincipal.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                                    _settings.Issuer,
                                    _settings.Audience,
                                    claims,
                                    expires: DateTime.UtcNow.AddHours(_settings.ExpiresHours),
                                    signingCredentials: cred
                                );

                #region only use for mobile
                var dealerOpeningsHierarchyList = await _commonService.GetPSTZHierarchy(userInfo.PlantIds, userInfo.SaleOfficeIds, userInfo.TerritoryIds, userInfo.ZoneIds);
                var painterRegistrationsHierarchyList = await _commonService.GetPTZHierarchy(userInfo.PlantIds, userInfo.TerritoryIds, userInfo.ZoneIds);
                //var leadGenerationsHierarchyList = await _commonService.GetPTZHierarchy(userInfo.PlantIds, userInfo.TerritoryIds, userInfo.ZoneIds);
                var leadGenerationsHierarchyList = painterRegistrationsHierarchyList;

                var plants = await _commonService.GetDepotList(x => (EnumEmployeeRole.Admin == userInfo.EmployeeRole || userInfo.EmployeeRole == EnumEmployeeRole.GM)
                                                                    || (userInfo.PlantIds != null && userInfo.PlantIds.Any(y => y == x.Werks)));
                var saleOffices = await _commonService.GetSaleOfficeList(x => (EnumEmployeeRole.Admin == userInfo.EmployeeRole || userInfo.EmployeeRole == EnumEmployeeRole.GM)
                                                                    || (userInfo.SaleOfficeIds != null && userInfo.SaleOfficeIds.Any(y => y == x.Code)));
                var areas = await _commonService.GetSaleGroupList(x => (EnumEmployeeRole.Admin == userInfo.EmployeeRole || userInfo.EmployeeRole == EnumEmployeeRole.GM)
                                                                    || (userInfo.AreaIds != null && userInfo.AreaIds.Any(y => y == x.Code)));
                var territories = await _commonService.GetTerritoryList(x => (EnumEmployeeRole.Admin == userInfo.EmployeeRole || userInfo.EmployeeRole == EnumEmployeeRole.GM)
                                                                    || (userInfo.TerritoryIds != null && userInfo.TerritoryIds.Any(y => y == x.Code)));
                var zones = await _commonService.GetZoneList(x => (EnumEmployeeRole.Admin == userInfo.EmployeeRole || userInfo.EmployeeRole == EnumEmployeeRole.GM)
                                                                    || (userInfo.ZoneIds != null && userInfo.ZoneIds.Any(y => y == x.Code)));
                #endregion

                var results = new AuthenticateUserModel()
                {
                    //userId=AppIdentity.AppUser.UserId,
                    //fullName=AppIdentity.AppUser.FullName,
                    UserId = userInfo.Id,
                    FullName = userInfo.FullName ?? string.Empty,
                    DealerOpeningsHierarchyList = dealerOpeningsHierarchyList, // only for app end
                    PainterRegistrationsHierarchyList = painterRegistrationsHierarchyList, // only for app end
                    LeadGenerationsHierarchyList = leadGenerationsHierarchyList, // only for app end
                    Plants = plants, // only for app end
                    SalesOffices = saleOffices, // only for app end
                    Areas = areas, // only for app end
                    Territories = territories, // only for app end
                    Zones = zones, // only for app end
                    PlantIds = userInfo.PlantIds,
                    PlantId = userInfo.PlantIds.FirstOrDefault() ?? string.Empty,
                    SalesOfficeIds = userInfo.SaleOfficeIds,
                    SalesOfficeId = userInfo.SaleOfficeIds.FirstOrDefault() ?? string.Empty,
                    AreaIds = userInfo.AreaIds,
                    AreaId = userInfo.AreaIds.FirstOrDefault() ?? string.Empty,
                    TerritoryIds = userInfo.TerritoryIds,
                    TerritoryId = userInfo.TerritoryIds.FirstOrDefault() ?? string.Empty,
                    ZoneIds = userInfo.ZoneIds,
                    ZoneId = userInfo.ZoneIds.FirstOrDefault() ?? string.Empty,
                    RoleId = userInfo.RoleId,
                    RoleName = userInfo.RoleName ?? string.Empty,
                    EmployeeId = userInfo.EmployeeId ?? string.Empty,
                    EmployeeRole = (int)userInfo.EmployeeRole,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    AppMenuPermission = empMenu.ToList()

                };

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<string>> GetDealerByUserId(int userId)
        {
            var result = await _commonService.AppGetDealerInfoListByCurrentUser(userId);
            return result.Select(x => x.CustomerNo).Distinct().ToList();
        }

        public AreaSearchCommonModel GetLoggedInUserArea()
        {
            var result = new AreaSearchCommonModel();
            var appUser = AppIdentity.AppUser;

            result.Depots = appUser.PlantIdList;
            result.SalesOffices = appUser.SalesOfficeIdList;
            result.SalesGroups = appUser.SalesAreaIdList;
            result.Territories = appUser.TerritoryIdList;
            result.Zones = appUser.ZoneIdList;

            return result;
        }
    }
}
