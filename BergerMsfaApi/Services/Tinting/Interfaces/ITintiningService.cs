using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Tinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Tinting.Interfaces
{
    public interface ITintiningService
    {
        Task<IPagedList<TintingMachineModel>> GetAllAsync(int index, int pageSize, string search);
        Task<QueryResultModel<TintingMachineModel>> GetAllAsync(QueryObjectModel query);
        Task<IList<SaveTintingMachineModel>> GetAllAsync(string territory, int userInfoId);
        Task<bool> UpdateAsync(List<SaveTintingMachineModel> model);
    }
}
