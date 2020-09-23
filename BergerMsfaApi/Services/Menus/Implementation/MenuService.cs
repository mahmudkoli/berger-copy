using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Menus;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Menus;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Menus.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menu;
        private readonly IRepository<MenuPermission> _menuPermission;


        public MenuService(IRepository<Menu> menu, IRepository<MenuPermission> menuPermission)
        {
            _menu = menu;
            _menuPermission = menuPermission;
        }

        public async Task<MenuModel> CreateAsync(MenuModel model)
        {
            var menu = model.ToMap<MenuModel, Menu>();
            var result = await _menu.CreateAsync(menu);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<MenuModel> CreateAndUpdateParentAsync(MenuModel model)
        {
            //shoud use transaction here
            //whenever a new menu is created, check whether it is being created under another parent
            //if the menu is being created under another parent, and the parent had no child before
            //parent data must be modified with the field isParent
            //that is=> parentMenu.isParent = true 

            var res = await CreateAsync(model);
            if (model.ParentId > 0)
            {
                var parent = await GetMenuAsync(model.ParentId);
                if (!parent.IsParent)
                {
                    parent.IsParent = true;
                    _ = await UpdateAsync(parent);
                }
            }

            return res;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var menuTobeDeleted = await GetMenuAsync(id);
            if (!menuTobeDeleted.IsParent)
            {
                var result = await _menu.DeleteAsync(s => s.Id == id);

                //update parent if deleted menu is the only child
                var siblings = await FindMenusByParentAsync(menuTobeDeleted.ParentId);
                if (siblings.Count() < 1)
                {
                    var parent = await GetMenuAsync(menuTobeDeleted.ParentId);
                    parent.IsParent = false;
                    _ = await UpdateAsync(parent);

                }

                return result;
            }
            else
            {
                throw new Exception("Parent Cannot be deleted.");
            }
        }

        private async Task<IEnumerable<MenuModel>> FindMenusByParentAsync(int parentId)
        {
            var result = await _menu.FindAllAsync(m => m.ParentId == parentId);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<bool> IsMenuExistAsync(string menuNamde, int id)
        {
            var result = id <= 0
                ? await _menu.IsExistAsync(s => s.Name == menuNamde)
                : await _menu.IsExistAsync(s => s.Name == menuNamde && s.Id != id);

            return result;
        }
        public async Task<MenuModel> GetMenuAsync(int id)
        {
            var result = await _menu.FindAsync(s => s.Id == id);
            return result.ToMap<Menu, MenuModel>();
        }

        // public async Task<IEnumerable<MenuModel>> GetMenusAsync()
        // {
        //     var result = (await _menu.GetAllAsync()).OrderByDescending(m => m.Id);
        //     return result.ToMap<Menu, MenuModel>();        
        // }

        public async Task<IEnumerable<MenuModel>> GetMenusAsync()
        {
            var result = _menu.GetAllIncludeStrFormat(includeProperties: "MenuPermissions.Role");

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Menu, MenuModel>();
                //.ForMember(m => m.MenuPermissions, opt => opt.MapFrom(x => x.MenuPermissions.Select(r => r.Role)));
                cfg.CreateMap<MenuPermission, MenuPermissionModel>();
                cfg.CreateMap<Role, RoleModel>();
            }).CreateMapper();

            var data = mapper.Map<List<MenuModel>>(result);

            List<MenuModel> hierarchyMenuData = new List<MenuModel>();
            hierarchyMenuData = data
                            .Where(c => c.ParentId == 0).OrderBy(o => o.Sequence)
                            .Select(c => new MenuModel()
                            {
                                Id = c.Id,
                                Status = c.Status,
                                Name = c.Name,
                                Controller = c.Controller,
                                Action = c.Action,
                                Url = c.Url,
                                IconClass = c.IconClass,
                                ParentId = c.ParentId,
                                IsParent = c.IsParent,
                                IsTitle = c.IsTitle,
                                Sequence = c.Sequence,
                                MenuPermissions = c.MenuPermissions,
                                Children = GetChildren(data, c.Id)
                            })
                            .ToList();

            return hierarchyMenuData;
        }

        public static List<MenuModel> GetChildren(List<MenuModel> menus, int parentId)
        {
            return menus
                    .Where(c => c.ParentId == parentId).OrderBy(o => o.Sequence)
                    .Select(c => new MenuModel
                    {
                        Id = c.Id,
                        Status = c.Status,
                        Name = c.Name,
                        Controller = c.Controller,
                        Action = c.Action,
                        Url = c.Url,
                        IconClass = c.IconClass,
                        ParentId = c.ParentId,
                        IsParent = c.IsParent,
                        IsTitle = c.IsTitle,
                        Sequence = c.Sequence,
                        MenuPermissions = c.MenuPermissions,
                        Children = GetChildren(menus, c.Id)
                    })
                    .Distinct().ToList();
        }

        public async Task<IEnumerable<MenuModel>> GetActiveMenusAsync()
        {
            var result = (await _menu.FindAllAsync(m => m.Status == Status.Active)).OrderByDescending(m => m.Id);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<IEnumerable<MenuModel>> GetChildMenusAsync()
        {
            var result = (await _menu.FindAllAsync(m => !m.IsParent)).OrderByDescending(m => m.Id);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<IPagedList<MenuModel>> GetPagedMenusAsync(int pageNumber, int pageSize)
        {
            var result = await _menu.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<Menu, MenuModel>();

        }

        public async Task<MenuModel> SaveAsync(MenuModel model)
        {
            var menu = model.ToMap<MenuModel, Menu>();
            var result = await _menu.CreateOrUpdateAsync(menu);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<MenuModel> UpdateAsync(MenuModel model)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuModel, Menu>()
                    .ForMember(src => src.MenuPermissions, opt => opt.Ignore());
            }).CreateMapper();

            var menu = mapper.Map<MenuModel, Menu>(model);
            var result = await _menu.UpdateAsync(menu);
            return result.ToMap<Menu, MenuModel>();
        }

        public async Task<List<MenuPermissionModel>> AssignRoleToMenuAsync(List<MenuPermissionModel> model, int roleId)
        {
            var result = new List<MenuPermission>();
            var menuPermission = model.ToMap<MenuPermissionModel, MenuPermission>();

            //Delete menu permissions
            var existingMenuPermissions = await GetMenuPermissionsByRoleId(roleId);
            var menuDeleteListModel = new List<MenuPermissionModel>();

            foreach (var menuPerm in existingMenuPermissions)
            {
                if (!menuPermission.Any(mp => mp.Id == menuPerm.Id))
                {
                    menuDeleteListModel.Add(menuPerm);
                }
            }

            var menuDeleteList = menuDeleteListModel.ToMap<MenuPermissionModel, MenuPermission>();
            await _menuPermission.DeleteListAsync(menuDeleteList);

            //Add/Update menu permissions            
            var menuCreateList = new List<MenuPermission>();
            var menuUpdateList = new List<MenuPermission>();
            foreach (var item in menuPermission)
            {
                var menuPermissionEntity = new MenuPermission()
                {
                    Id = item.Id,
                    MenuId = item.MenuId,
                    RoleId = item.RoleId
                };

                if (item.Id == 0)
                {
                    menuCreateList.Add(menuPermissionEntity);
                }
                // else
                // {
                //     menuUpdateList.Add(menuPermissionEntity);
                // }
            }

            if (menuCreateList.Count != 0)
            {
                var resultCreate = await _menuPermission.CreateListAsync(menuCreateList);
                result.AddRange(resultCreate);
            }
            // if (menuUpdateList.Count != 0)
            // {
            //     var resultUpdate = await _menuPermission.UpdateListAsync(menuUpdateList);
            //     result.AddRange(resultUpdate);
            // }

            var data = result.ToMap<MenuPermission, MenuPermissionModel>();
            return data;
        }

        public async Task<List<MenuPermissionModel>> GetMenuPermissionsByRoleId(int roleId)
        {
            try
            {
                var result = await _menuPermission.FindAllAsync(mp => mp.RoleId == roleId);
                return result.ToMap<MenuPermission, MenuPermissionModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MenuModel>> GetPermissionMenus(int roleId)
        {
            var result = _menuPermission.FindAllInclude(mp => mp.RoleId == roleId, mp => mp.Menu).Select(x => x.Menu).ToList();
            var allMenus = _menu.GetAll().ToList();

            var data = result.ToMap<Menu, MenuModel>();
            var allMenuData = allMenus.ToMap<Menu, MenuModel>();

            data = GetParentMenu(allMenuData, data).Menus;

            List<MenuModel> hierarchyMenuData = new List<MenuModel>();
            hierarchyMenuData = data
                            .Where(c => c.ParentId == 0).OrderBy(o => o.Sequence)
                            .Select(c => new MenuModel()
                            {
                                Id = c.Id,
                                Status = c.Status,
                                Name = c.Name,
                                Controller = c.Controller,
                                Action = c.Action,
                                Url = c.Url,
                                IconClass = c.IconClass,
                                ParentId = c.ParentId,
                                IsParent = c.IsParent,
                                IsTitle = c.IsTitle,
                                Sequence = c.Sequence,
                                MenuPermissions = c.MenuPermissions,
                                Children = GetChildren(data, c.Id)
                            })
                            .ToList();

            return hierarchyMenuData;
        }

        public (List<MenuModel> AllMenus, List<MenuModel> Menus) GetParentMenu(List<MenuModel> allMenus, List<MenuModel> menus)
        {
            var newList = new List<MenuModel>(menus);

            foreach (var item in menus.Where(x => x.ParentId != 0))
            {
                if (!newList.Any(x => x.Id == item.ParentId))
                {
                    var data = allMenus.Find(x => x.Id == item.ParentId);
                    newList.Add(data);
                }
            }

            if (!newList.All(m => m.ParentId == 0 || newList.Any(mp => mp.Id == m.ParentId)))
                newList = GetParentMenu(allMenus, newList).Menus;

            return (allMenus.Distinct().ToList(), newList.Distinct().ToList());
        }

    }
}
