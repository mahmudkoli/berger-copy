using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Logs;
using BergerMsfaApi.Models.Tinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Logs.Interfaces
{
    public interface IApplicationLogService
    {
        Task<bool> AddMobileAppLogAsync(MobileAppLogModel model);
    }
}
