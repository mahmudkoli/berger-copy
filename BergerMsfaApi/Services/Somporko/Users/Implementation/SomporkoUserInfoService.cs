using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Somporko.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BergerMsfaApi.Services.Somporko.Users.Implementation
{
    public class SomporkoUserInfoService : ISomporkoUserInfoService
    {
        private readonly IRepository<UserInfo> _userInfoRepo;
        private readonly IRepository<UserZoneAreaMapping> _userZoneAreaRepo;
        private readonly IRepository<UserRoleMapping> _userRoleMappingRepo;
        private readonly IMapper _mapper;


        public SomporkoUserInfoService(
            IRepository<UserInfo> userInfoRepo,
            IRepository<UserZoneAreaMapping> userZoneAreaRepo,
            IRepository<UserRoleMapping> userRoleMappingRepo,
            IMapper mapper)
        {
            _userInfoRepo = userInfoRepo;
            _userRoleMappingRepo = userRoleMappingRepo;
            _userZoneAreaRepo = userZoneAreaRepo;
            _mapper = mapper;
        }

        public async Task<QueryResultModel<UserInfoModel>> GetUsersAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<UserInfo, object>>>()
            {
                ["userName"] = v => v.UserName,
                ["fullName"] = v => v.FullName,
                ["designation"] = v => v.Designation,
                ["department"] = v => v.Department,
            };

            var result = await _userInfoRepo.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.FullName.Contains(query.GlobalSearchValue) || x.Department.Contains(query.GlobalSearchValue) || x.Designation.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.Roles).ThenInclude(i => i.Role),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<UserInfoModel>>(result.Items);

            var queryResult = new QueryResultModel<UserInfoModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }
    }
}
