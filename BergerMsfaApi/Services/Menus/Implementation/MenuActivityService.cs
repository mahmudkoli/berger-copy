using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Data.MsfaEntity.Menus;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Menus;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Menus.Interfaces;
using Microsoft.Data.SqlClient;
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Implementation
{
    public class MenuActivityService : IMenuActivityService
    {
        private readonly IRepository<MenuActivity> _menuActivity;
        private readonly IRepository<MenuPermission> _menuPermission;

        private readonly IRepository<MenuActivityPermission> _menuActivityPermission;
        public MenuActivityService(IRepository<MenuActivity> menuActivity, IRepository<MenuPermission> menuPermission, IRepository<MenuActivityPermission> menuActivityPermission)
        {
            _menuActivity = menuActivity;
            _menuPermission = menuPermission;
            _menuActivityPermission = menuActivityPermission;
        }

        public Task<MenuActivityModel> CreateAndUpdateParentAsync(MenuActivityModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuActivityModel> CreateAsync(MenuActivityModel model)
        {
            var menuActivity = model.ToMap<MenuActivityModel, MenuActivity>();
            var result = await _menuActivity.CreateAsync(menuActivity);
            return result.ToMap<MenuActivity, MenuActivityModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _menuActivity.DeleteAsync(s => s.Id == id);
            return result;
        }

        public async Task<MenuActivityModel> GetMenuActivityAsync(int id)
        {
            var result = await _menuActivity.FindAsync(s => s.Id == id);
            return result.ToMap<MenuActivity, MenuActivityModel>();
        }

        public async Task<IEnumerable<MenuActivityModel>> GetAllMenusActivityAsync()
        {
            IQueryable<MenuActivity> result = _menuActivity.GetAllInclude(m => m.Menu);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuActivity, MenuActivityModel>();
                cfg.CreateMap<Menu, MenuModel>();
            }).CreateMapper();
            var data = mapper.Map<List<MenuActivityModel>>(result).ToList();

            return data;
        }

        public Task<IPagedList<MenuActivityModel>> GetPagedMenusAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsMenuActivityExistAsync(string name, string code, int id)
        {
            var result = id <= 0
                ? await _menuActivity.IsExistAsync(s => s.Name == name && s.ActivityCode == code)
                : await _menuActivity.IsExistAsync(s => s.Name == name && s.ActivityCode == code && s.Id != id);

            return result;
        }

        public Task<MenuActivityModel> SaveAsync(MenuActivityModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuActivityModel> UpdateAsync(MenuActivityModel model)
        {
            var menu = model.ToMap<MenuActivityModel, MenuActivity>();
            var result = await _menuActivity.UpdateAsync(menu);
            return result.ToMap<MenuActivity, MenuActivityModel>();
        }

        public async Task<IEnumerable<MenuActivityModel>> GetAllMenuActivityById(int id)
        {
            var result = await _menuActivity.FindAllAsync(x => x.MenuId == id);
            return result.ToMap<MenuActivity, MenuActivityModel>();
        }

        public async Task<IEnumerable<MenuActivityPermissionVm>> GetAllMenuActivityPermissionByRoleId(int id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@RoleId", id);
                var result = await _menuActivity.ExecuteQueryAsyc<MenuActivityPermissionVm>("exec [dbo].[sp_GetRoleWiseMenuActivityWithPermission] @RoleId", param);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }


            /*var menuPermissionsData = await _menuPermission.FindAllAsync(x => x.RoleId == id);
            var menuPermissionList = menuPermissionsData.ToMap<MenuPermission, MenuPermissionModel>();

            var menuActivityList = new List<MenuActivityModel>();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuActivity, MenuActivityModel>();
                cfg.CreateMap<Menu, MenuModel>();
            }).CreateMapper();

            foreach (var menuPermission in menuPermissionList)
            {

                var menuActivityListData = _menuActivity.FindAllInclude(m => m.MenuId == menuPermission.MenuId, m => m.Menu);

                var tempMenuActivityList = mapper.Map<List<MenuActivityModel>>(menuActivityListData).ToList();

                foreach (var menuActivity in tempMenuActivityList)
                {
                    var menuActivityPermission = await _menuActivityPermission.FindAsync(m => m.RoleId == id && m.ActivityId == menuActivity.Id);

                    if (menuActivityPermission != null)
                    {
                        menuActivity.MenuActivityPermission = menuActivityPermission.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
                    }
                    else
                    {

                        var tempMenuActivityPermission = new MenuActivityPermissionModel()
                        {


                            RoleId = id,
                            ActivityId = menuActivity.Id,
                            CanView = false,
                            CanInsert = false,
                            CanUpdate = false,
                            CanDelete = false

                        };

                        menuActivity.MenuActivityPermission = tempMenuActivityPermission;


                    }


                }

                menuActivityList.AddRange(tempMenuActivityList);


            }*/

        }
    }
}
