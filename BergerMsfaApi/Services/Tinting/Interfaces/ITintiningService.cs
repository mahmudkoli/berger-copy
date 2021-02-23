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
        Task<IPagedList<TintingMachineModel>> GetTintingMachinePagingList(int index, int pageSize, string search);
        Task<IList<SaveTintingMachineModel>> AppGetTintingMachineList(string territory, int userInfoId);
        Task<bool> AppUpdateTitningMachine(List<SaveTintingMachineModel> model);
    }
}
