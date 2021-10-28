using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Logs;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Tinting;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Logs;
using BergerMsfaApi.Models.Tinting;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Logs.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using BergerMsfaApi.Services.Tinting.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Logs.Implementation
{
    public class ApplicationLogService : IApplicationLogService
    {
        public readonly IRepository<MobileAppLog> _mobileAppLogSvc;
        public readonly IMapper _mapper;

        public ApplicationLogService(
              IRepository<MobileAppLog> mobileAppLogSvc,
              IMapper mapper
            )
        {
            _mobileAppLogSvc = mobileAppLogSvc;
            _mapper = mapper;
        }

        public async Task<bool> AddMobileAppLogAsync(MobileAppLogModel model)
        {
            try
            {
                var entity = _mapper.Map<MobileAppLog>(model);
                await _mobileAppLogSvc.CreateAsync(entity);
                await _mobileAppLogSvc.SaveChangesAsync();
            }
            catch (Exception)
            {
            }

            return true;
        }
    }
}
