using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Domain.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _role;

        public RoleService(IRepository<Role> example)
        {
            _role = example;
        }


        public async Task<RoleModel> CreateAsync(RoleModel model)
        {
            var role = model.ToMap<RoleModel, Role>();
            var result = await _role.CreateAsync(role);
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _role.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsRoleExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _role.IsExistAsync(s => s.Name == name)
                : await _role.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }
        public async Task<bool> IsRoleNameExistAsync(string name)
        {
            return await _role.IsExistAsync(s => s.Name == name);
        }
        public async Task<RoleModel> GetRoleAsync(int id)
        {
            var result = await _role.FindAsync(s => s.Id == id);
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            var result = await _role.GetAllAsync();
            result.OrderBy(a => a.CreatedTime);
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<IPagedList<RoleModel>> GetPagedRolesAsync(int pageNumber, int pageSize)
        {
            var result = await _role.GetAllPagedAsync(pageNumber, pageSize);
            result.OrderBy(a => a.CreatedTime);
            return result.ToMap<Role, RoleModel>();

        }

        public async Task<IEnumerable<RoleModel>> GetQueryRolesAsync()
        {
            var result = await _role.ExecuteQueryAsyc<RoleModel>("SELECT * FROM Roles");
            return result;
        }

        public async Task<RoleModel> SaveAsync(RoleModel model)
        {
            var role = model.ToMap<RoleModel, Role>();
            var result = await _role.CreateOrUpdateAsync(role);
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<RoleModel> UpdateAsync(RoleModel model)
        {
            var role = model.ToMap<RoleModel, Role>();
            var result = await _role.UpdateAsync(role);
            return result.ToMap<Role, RoleModel>();
        }


    }
}
