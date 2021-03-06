using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Somporko.Users;
using BergerMsfaApi.Models.Users;
using X.PagedList;

namespace BergerMsfaApi.Services.Somporko.Users.Interfaces
{
    public interface ISomporkoUserInfoService
    {
        Task<IList<SomporkoUserInfoModel>> GetUsersAsync();
    }
}
