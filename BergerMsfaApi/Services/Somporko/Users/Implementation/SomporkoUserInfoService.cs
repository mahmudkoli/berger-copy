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
using BergerMsfaApi.Models.Somporko.Users;
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

        public async Task<IList<SomporkoUserInfoModel>> GetUsersAsync()
        {
            var result = await _userInfoRepo.GetAllIncludeAsync(
                                x => x,
                                x => true,
                                x => x.OrderBy(o => o.FullName),
                                x => x.Include(i => i.UserZoneAreaMappings),
                                true
                            );

            var modelResult = _mapper.Map<IList<SomporkoUserInfoModel>>(result);

            return modelResult;
        }
    }
}
