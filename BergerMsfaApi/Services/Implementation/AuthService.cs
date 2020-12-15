using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Core;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BergerMsfaApi.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly IUserInfoService _userService;
        private readonly ICMUserService _cmUserService;

        public AuthService(IConfiguration config, IUserInfoService user, ICMUserService cmUser)
        {
            configuration = config;
            _userService = user;
            this._cmUserService = cmUser;
        }

        public async Task<object> GetJWTTokenByUserNameAsync(string userName)
        {
            try
            {
                ////var cmUser = await _cmUserService.GetCMUserByLogin(model);

                //var user = new AppUserPrincipal("brainstation23")
                //{
                //    UserId = 1,
                //    Email = "abc@abc.com",
                //    ActiveRoleId = 1,
                //    RoleIdList = new List<int> { 0 },
                //    Avatar = "/img/user.png",
                //    FullName = "Full Name",
                //    EmployeeId = "0",
                //    Phone = "011121",
                //    UserAgentInfo = "127.0.0.1",
                //};

                //var cmUser = await _cmUserService.GetCMUserByLogin(model);
                //var user = new AppUserPrincipal(model.MobileNumber)
                //{
                //    UserId =AppIdentity.AppUser.UserId,
                //    Email = AppIdentity.AppUser.Email,
                //    ActiveRoleId = AppIdentity.AppUser.ActiveRoleId,
                //    RoleIdList = AppIdentity.AppUser.RoleIdList,
                //    Avatar = "/img/user.png",
                //    FullName = AppIdentity.AppUser.FullName,
                //    EmployeeId = AppIdentity.AppUser.EmployeeId,
                //    Phone = AppIdentity.AppUser.Phone,
                //    UserAgentInfo = AppIdentity.AppUser.UserAgentInfo

                //};

                var userInfo = await _userService.GetUserByUserNameAsync(userName);

                var user = new AppUserPrincipal(userInfo.UserName)
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    ActiveRoleId = userInfo.RoleId,
                    RoleIdList = userInfo.RoleIds,
                    Avatar = "/img/user.png",
                    FullName = $"{userInfo.FirstName} {userInfo.MiddleName} {userInfo.LastName}",
                    EmployeeId = userInfo.EmployeeId,
                    Phone = userInfo.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",
                };

                var appClaimes = user
                                .GetByName()
                                .Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                                {
                                        new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                                        new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                                };
                claims.AddRange(appClaimes);

                foreach (var role in user.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                                    configuration["Tokens:Issuer"],
                                    configuration["Tokens:Audience"],
                                    claims,
                                    expires: DateTime.UtcNow.AddHours(1.00),
                                    signingCredentials: cred
                                );

                #region user category
                var userCat = string.Empty;
                var userCatIds = new List<string>();

                if (userInfo.zoneIds.Any())
                {
                    userCat = EnumUserCategory.Zone.ToString();
                    userCatIds = userInfo.zoneIds;
                } 
                else if (userInfo.territoryIds.Any())
                {
                    userCat = EnumUserCategory.Territory.ToString();
                    userCatIds = userInfo.territoryIds;
                } 
                else if (userInfo.areaIds.Any())
                {
                    userCat = EnumUserCategory.Area.ToString();
                    userCatIds = userInfo.areaIds;
                } 
                else if (userInfo.saleOfficeIds.Any())
                {
                    userCat = EnumUserCategory.SalesOffice.ToString();
                    userCatIds = userInfo.saleOfficeIds;
                } 
                else if (userInfo.plantIds.Any())
                {
                    userCat = EnumUserCategory.Plant.ToString();
                    userCatIds = userInfo.plantIds.Select(x => x.ToString()).ToList();
                }
                #endregion

                var results = new
                {
                    //userId=AppIdentity.AppUser.UserId,
                    //fullName=AppIdentity.AppUser.FullName,
                    userId = userInfo.Id,
                    fullName = $"{userInfo.FirstName} {userInfo.MiddleName} {userInfo.LastName}",
                    planIds = userInfo.plantIds,
                    planId = userInfo.plantIds.FirstOrDefault(),
                    salesOfficeIds = userInfo.saleOfficeIds,
                    salesOfficeId = userInfo.saleOfficeIds.FirstOrDefault()??"",
                    areaIds = userInfo.areaIds,
                    areaId = userInfo.areaIds.FirstOrDefault()??"",
                    territoryIds = userInfo.territoryIds,
                    territoryId = userInfo.territoryIds.FirstOrDefault()??"",
                    zoneIds = userInfo.zoneIds,
                    zoneId = userInfo.zoneIds.FirstOrDefault()??"",
                    UserCategory = userCat,
                    UserCategoryIds = userCatIds,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    roleId=userInfo.RoleId,
                    employeeId = userInfo.EmployeeId,
                };

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<object> GetJWTToken(PortalLoginModel model)
        {
            try
            {
                var userInfo = await _userService.GetCurrentUser(model.UserName);                
                

                var user = new AppUserPrincipal("brainstation23")
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    ActiveRoleId = userInfo.RoleId,
                    RoleIdList = userInfo.RoleIds,
                    Avatar = "/img/user.png",
                    FullName = userInfo.Name,
                    EmployeeId = userInfo.EmployeeId,
                    Phone = userInfo.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",
                    NodeId = userInfo.NodeId,
                    ActiveRoleName = userInfo.RoleName

                };
                var appClaimes = user
                                .GetByName()
                                .Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                    {

                            new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                claims.AddRange(appClaimes);
                foreach (var role in user.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

                }


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Tokens:Issuer"],
                    configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(15.00),
                    signingCredentials: cred

                    );


                var results = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };

                return results;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }



    }
}
