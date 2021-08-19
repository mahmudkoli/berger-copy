using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface IUserInfoService
    {
        Task<QueryResultModel<UserInfoModel>> GetUsersAsync(QueryObjectModel query);
        Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync();
        Task<UserInfoModel> GetUserAsync(int id);
        Task<UserInfoModel> GetUserByUserNameAsync(string username);
        Task<UserInfoModel> SaveAsync(UserInfoModel model);
        Task<UserRoleMappingModel> SaveRoleLinkWithUserAsync(UserRoleMappingModel model);
        Task<UserInfoModel> CreateAsync(SaveUserInfoModel model);
        Task<UserRoleMappingModel> CreateRoleLinkWithUserAsync(UserRoleMappingModel model);
        Task<UserInfoModel> UpdateAsync(SaveUserInfoModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsUserExistAsync(string code, int id);
        Task<bool> IsUserNameExistAsync(string username, int id = 0);
        Task<bool> IsActiveUserAsync(string username, int id = 0);
        Task<bool> IsRoleLinkWithUserExistAsync(int roleId, int userInfoId);
        Task<bool> IsRoleMappingExistAsync(int userInfoId);
        Task<bool> IsUserExistAsync(string adguid);
        Task<Nullable<int>> GetFMUserIdByNameAsync(string name);
        Task<Nullable<int>> GetFMUserIdByEmailAsync(string email);
        Task<UserRoleMappingModel> GetUserRoleMappingByUserInfoIdAsync(int userinfoId);
        Task<int> DeleteUserRoleMappingAsync(int id);
    }
}
