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
using X.PagedList;

namespace BergerMsfaApi.Services.Menus.Implementation
{
    public class MenuActivityPermissionService : IMenuActivityPermissionService
    {
        private readonly IRepository<MenuActivityPermission> _repository;
        public MenuActivityPermissionService(IRepository<MenuActivityPermission> repository)
        {
            _repository = repository;


        }
        public Task<MenuActivityPermissionModel> CreateAndUpdateParentAsync(MenuActivityPermissionModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuActivityPermissionModel> CreateAsync(MenuActivityPermissionModel model)
        {
            var menuActivityPermission = model.ToMap<MenuActivityPermissionModel, MenuActivityPermission>();
            var result = await _repository.CreateAsync(menuActivityPermission);
            return result.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(s => s.Id == id);
            return result;
        }

        public async Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionAsync()
        {
            var result = await _repository.GetAllAsync();
            return result.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
        }

        public async Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionByRoleIdAsync(int id)
        {
            var result = await _repository.FindAllAsync(s => s.RoleId == id);
            return result.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
        }

        public async Task<MenuActivityPermissionModel> GetMenuActivityPermissionAsync(int id)
        {
            var result = await _repository.FindAsync(s => s.Id == id);
            return result.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
        }

        public Task<IPagedList<MenuActivityPermissionModel>> GetPagedMenuPermissionAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsMenuActivityPermissionExistAsync(int roleId, int id)
        {
            var result = id <= 0
              ? await _repository.IsExistAsync(s => s.RoleId == roleId)
              : await _repository.IsExistAsync(s => s.RoleId == roleId && s.Id != id);

            return result;
        }

        public Task<MenuActivityPermissionModel> SaveAsync(MenuActivityPermissionModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuActivityPermissionModel> UpdateAsync(MenuActivityPermissionModel model)
        {
            var menu = model.ToMap<MenuActivityPermissionModel, MenuActivityPermission>();
            var result = await _repository.UpdateAsync(menu);
            return result.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();
        }

        public async Task<List<MenuActivityPermissionModel>> CreateOrUpdateAllAsync(List<MenuActivityPermissionVm> modelList)
        {

            List<MenuActivityPermission> createList = new List<MenuActivityPermission>();
            List<MenuActivityPermission> updateList = new List<MenuActivityPermission>();

            foreach (var item in modelList)
            {
                 MenuActivityPermission map = new MenuActivityPermission()
                    {
                        Id = item.Id,
                        RoleId = item.RoleId,
                        ActivityId = item.ActivityId,
                        CanView = item.CanView,
                        CanUpdate = item.CanUpdate,
                        CanInsert = item.CanInsert,
                        CanDelete = item.CanDelete

                    };

                if (item.Id <= 0)
                {
                    createList.Add(map);
                }
                else
                {
                    updateList.Add(map);
                }
            }  


            if (createList.Count > 0)
            {
                createList = await _repository.CreateListAsync(createList);
            }

            if (updateList.Count > 0)
            {
                updateList = await _repository.UpdateListAsync(updateList);
            }


            var finalResult = new List<MenuActivityPermission>();
            finalResult.AddRange(createList);
            finalResult.AddRange(updateList);

            return finalResult.ToMap<MenuActivityPermission, MenuActivityPermissionModel>();

        }

        public async Task<List<ActivityPermissionModel>> GetActivityPermissions(int roleId)
        {
            var result = _repository.GetAllIncludeStrFormat(filter: s => s.RoleId == roleId,
                includeProperties: "Role,Activity,Activity.Menu").ToList();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuActivityPermission, ActivityPermissionModel>()
                    .ForMember(src => src.RoleName, opt => opt.MapFrom(dest => dest.Role.Name))
                    .ForMember(src => src.ActivityName, opt => opt.MapFrom(dest => dest.Activity.Name))
                    .ForMember(src => src.ActivityCode, opt => opt.MapFrom(dest => dest.Activity.ActivityCode))
                    .ForMember(src => src.Url, opt => opt.MapFrom(dest => dest.Activity.Menu.Url))
                    .ForMember(src => src.MenuId, opt => opt.MapFrom(dest => dest.Activity.Menu.Id));
            }).CreateMapper();

            return mapper.Map<List<MenuActivityPermission>, List<ActivityPermissionModel>>(result);
        }
    }
}
