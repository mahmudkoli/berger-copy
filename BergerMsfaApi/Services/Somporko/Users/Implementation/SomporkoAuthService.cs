using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Core;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Somporko.Users;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Menus.Interfaces;
using BergerMsfaApi.Services.Somporko.Users.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BergerMsfaApi.Services.Somporko.Users.Implementation
{
    public class SomporkoAuthService : ISomporkoAuthService
    {
        private readonly TokensSettingsModel _tokenSettings;
        private readonly AppSettingsModel _appSettings;
        private readonly IRepository<UserInfo> _userRepo;

        public SomporkoAuthService(
            IOptions<TokensSettingsModel> tokenSettings,
            IOptions<AppSettingsModel> appSettings,
            IRepository<UserInfo> userRepo)
        {
            _tokenSettings = tokenSettings.Value;
            _appSettings = appSettings.Value;
            _userRepo = userRepo;
        }

        public async Task<bool> IsUserExistAsync(string username, string password)
        {
            var encryptPassword = SecurityExtension.ToEncryptString(password, _appSettings.ExternalAppSecurityKey);
            return await _userRepo.IsExistAsync(s => s.ApplicationCategory != EnumApplicationCategory.MSFAApp && s.UserName.ToLower() == username.ToLower() && s.Password == encryptPassword, true);
        }

        public async Task<bool> IsActiveUserAsync(string username)
        {
            return await _userRepo.IsExistAsync(s => s.ApplicationCategory != EnumApplicationCategory.MSFAApp && s.Status == Status.Active && s.UserName.ToLower() == username.ToLower(), true);
        }

        public async Task<SomporkoAuthenticateUserModel> GetJWTTokenByUserNameAsync(string userName)
        {
            try
            {
                var userInfo = await _userRepo.FindAsync(x => x.ApplicationCategory != EnumApplicationCategory.MSFAApp && x.UserName.ToLower() == userName.ToLower(), true);

                var userPrincipal = new AppUserPrincipal(userInfo.UserName)
                {
                    UserId = userInfo.Id,
                    FullName = userInfo.FullName
                };

                var appClaimes = userPrincipal.GetByName().Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName,userPrincipal.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub,userPrincipal.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(ConstantsApplication.ApplicationCategory, userInfo.ApplicationCategory.ToString()),
                };
                claims.AddRange(appClaimes);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                                    _tokenSettings.Issuer,
                                    _tokenSettings.Audience,
                                    claims,
                                    expires: DateTime.Now.AddHours(_tokenSettings.ExternalAppExpiresHours),
                                    signingCredentials: cred
                                );

                var results = new SomporkoAuthenticateUserModel()
                {
                    UserName = userInfo.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
