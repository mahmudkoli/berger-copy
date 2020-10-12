using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class CMUserService : ICMUserService
    {
        private readonly IRepository<CMUser> _user;
        private readonly IUserInfoService _userInfoService;

        public CMUserService(IRepository<CMUser> user, IUserInfoService userInfoService)
        {
            _user = user;
            _userInfoService = userInfoService;
        }

        public async Task<UserViewModel> CreateUserAsync(UserViewModel model)
        {
            var user = model.ToMap<UserViewModel, CMUser>();
        
            var result = await _user.CreateAsync(user);
            return result.ToMap<CMUser, UserViewModel>();

        }

        public async Task<bool> LoginCMUser(LoginModel model)
        {
            var isLoggedIn = await _user.AnyAsync(a => a.PhoneNumber == model.MobileNumber && a.Password == model.Password);
            return isLoggedIn;
        }

        public async Task<UserViewModel> GetCMUserByLogin(LoginModel model)
        {
            var result = await _user.FindAsync(a => a.PhoneNumber == model.MobileNumber && a.Password == model.Password);
            return result.ToMap<CMUser, UserViewModel>();
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUserAsync()
        {
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            var result = new List<CMUser>();
            if(isAdmin)
            {
                result = _user.GetAll().ToList();
            }
            else
            {
                var userIds = _user.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                result = _user.FindAll(x => userIds.Contains(x.FMUserId ?? 0)).ToList();
            }

            return result.ToMap<CMUser, UserViewModel>();
        }

        public async Task<IEnumerable<UserViewModel>> GetAllCMUsersByCurrentUserIdAsync()
        {


            var appUserId = AppIdentity.AppUser.UserId;

            var result = new List<CMUser>();
            
            
            var userIds = _user.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            result = _user.FindAll(x => userIds.Contains(x.FMUserId ?? 0)).ToList();
            

            return result.ToMap<CMUser, UserViewModel>();
            
        }

        public async Task<UserViewModel> GetUserAsync(int id)
        {
            var result = await _user.FindAsync(x => x.Id == id);
            return result.ToMap<CMUser, UserViewModel>();
        }

        public async Task<UserViewModel> UpdateAsync(UserViewModel model)
        {
            var user = model.ToMap<UserViewModel, CMUser>();
            var result = await _user.UpdateAsync(user);
            return result.ToMap<CMUser, UserViewModel>();
        }
        public async Task<UserViewModel> SaveAsync(UserViewModel model)
        {
            var example = model.ToMap<UserViewModel, CMUser>();
            var result = await _user.CreateOrUpdateAsync(example);
            return result.ToMap<CMUser, UserViewModel>();

        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _user.DeleteAsync(s => s.Id == id);
            return result;
        }


        public async Task<bool> IsUserExistAsync(string email, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.Email == email)
                : await _user.IsExistAsync(s => s.Email == email && s.Id != id);

            return result;
        }

        public async Task<IEnumerable<UserViewModel>> GetCMUserByFMIdAsync(int id)
        {
            var result = await _user.FindAllAsync(cm => cm.FMUserId == id);
            return result.ToMap<CMUser, UserViewModel>();

        }

        public async Task<(IEnumerable<UserViewModel> Data, string Message)> ExcelSaveToDatabaseAsync(DataTable datatable)
        {
            var items = new List<CMUser>();
            var existCount = 0;

            foreach (DataRow row in datatable.Rows)
            {
                var item = new CMUser();
                item.Name = row["NAME"].ObjectToString("NULL");
                item.Email = row["EMAIL"].ObjectToString("NULL");
                item.Password = row["PASSWORD"].ObjectToString("NULL");
                item.PhoneNumber = row["MOBILENUMBER"].ObjectToString("NULL");
                item.FamilyContactNo = row["FAMILYCONTACTNO"].ObjectToString("NULL");
                item.Address = row["ADDRESS"].ObjectToString("NULL");
                item.Status = int.TryParse(row["STATUS"].ToString(), out var st) ? 
                    (st == (int)Status.Active ? Status.Active : Status.InActive) : Status.InActive;

                var fmEmail = row["SUPERVISOR"].ObjectToString("NULL");
                var fmUserId = await _userInfoService.GetFMUserIdByEmailAsync(fmEmail);
                item.FMUserId = fmUserId;

                var isExist = _user.IsExist(x => x.PhoneNumber == item.PhoneNumber);
                if(isExist)
                {
                    existCount++;
                    continue;
                }

                items.Add(item);
            }

            var result = await _user.CreateListAsync(items);

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CMUser, UserViewModel>()
                    .ForMember(src => src.Password, opt => opt.Ignore());
            }).CreateMapper();

            var data = mapper.Map<List<UserViewModel>>(result);

            var message = items.Count + " new data saved" + 
                (existCount > 0 ? " and " + existCount + " data already exists." : ".");

            return (data, message);
        }
    }
}
