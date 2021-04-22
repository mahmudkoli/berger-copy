using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Core;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Interfaces;
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
        private readonly ICommonService _commonService;

        public AuthService(
            IOptions<TokensSettingsModel> settings,
            IUserInfoService user,
            ICommonService commonService)
        {
            _settings = settings.Value;
            _userService = user;
            _commonService = commonService;
        }

        public async Task<AuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName)
        {
            try
            {
                var userInfo = await _userService.GetUserByUserNameAsync(userName);

                var userPrincipal = new AppUserPrincipal(userInfo.UserName)
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    ActiveRoleId = userInfo.RoleId,
                    RoleIdList = userInfo.RoleIds,
                    Avatar = "/img/user.png",
                    FullName = $"{userInfo.FullName}",
                    EmployeeId = userInfo.EmployeeId,
                    Phone = userInfo.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",
                    //NodeId = userInfo.NodeId,
                    ActiveRoleName = userInfo.RoleName,
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

                #region user category
                var userCat = string.Empty;
                var userCatIds = new List<string>();

                userCat = UserCat(userInfo, userCat, ref userCatIds);
                #endregion

                var dealerOpeningsHierarchyList = await _commonService.GetPSATZHierarchy(userInfo.PlantIds, userInfo.SaleOfficeIds, userInfo.AreaIds, userInfo.TerritoryIds, userInfo.ZoneIds);
                var painterRegistrationsHierarchyList = await _commonService.GetPATZHierarchy(userInfo.PlantIds, userInfo.AreaIds, userInfo.TerritoryIds, userInfo.ZoneIds);
                var leadGenerationsHierarchyList = await _commonService.GetPTZHierarchy(userInfo.PlantIds, userInfo.TerritoryIds, userInfo.ZoneIds);


                var results = new AuthenticateUserModel()
                {
                    //userId=AppIdentity.AppUser.UserId,
                    //fullName=AppIdentity.AppUser.FullName,
                    UserId = userInfo.Id,
                    FullName = userInfo.FullName ?? string.Empty,
                    Plants = leadGenerationsHierarchyList,
                    DealerOpeningsHierarchyList = dealerOpeningsHierarchyList,
                    PainterRegistrationsHierarchyList = painterRegistrationsHierarchyList,
                    LeadGenerationsHierarchyList = leadGenerationsHierarchyList,
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
                    UserCategory = userCat ?? string.Empty,
                    UserCategoryIds = userCatIds,
                    RoleId = userInfo.RoleId,
                    RoleName = userInfo.RoleName ?? string.Empty,
                    EmployeeId = userInfo.EmployeeId ?? string.Empty,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                };

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<int>> GetDealerByUserId(int userId)
        {

            //var userInfo = await _userService.GetUserAsync(userId);
            //var userCat = string.Empty;
            //var userCatIds = new List<string>();

            //userCat = UserCat(userInfo, userCat, ref userCatIds);

            //var result = await _commonService.AppGetDealerInfoListByUserCategory(userCat, userCatIds);
            var result = await _commonService.AppGetDealerInfoListByCurrentUser();
            return result.Select(x => x.CustomerNo).Distinct().ToList();
        }

        private  string UserCat(UserInfoModel userInfo, string userCat, ref List<string> userCatIds)
        {
            switch (userInfo.EmployeeRole)
            {
                case EnumEmployeeRole.DIC:
                    userCat = EnumUserCategory.Plant.ToString();
                    userCatIds = userInfo.PlantIds.Select(x => x.ToString()).ToList();
                    break;
                case EnumEmployeeRole.BIC:
                    userCat = EnumUserCategory.SalesOffice.ToString();
                    userCatIds = userInfo.SaleOfficeIds.Select(x => x.ToString()).ToList();
                    break;
                case EnumEmployeeRole.AM:
                    userCat = EnumUserCategory.Area.ToString();
                    userCatIds = userInfo.AreaIds.Select(x => x.ToString()).ToList();
                    break;
                case EnumEmployeeRole.TM_TO:
                    userCat = EnumUserCategory.Territory.ToString();
                    userCatIds = userInfo.TerritoryIds.Select(x => x.ToString()).ToList();
                    break;
                case EnumEmployeeRole.ZO:
                    userCat = EnumUserCategory.Zone.ToString();
                    userCatIds = userInfo.ZoneIds.Select(x => x.ToString()).ToList();
                    break;
                default:
                    break;
            }

            return userCat;
        }
    }
}
