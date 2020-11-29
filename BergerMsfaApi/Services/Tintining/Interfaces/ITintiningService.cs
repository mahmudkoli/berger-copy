using BergerMsfaApi.Models.Tintining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Tintining.Interfaces
{
    public interface ITintiningService
    {
        #region App
        Task<IEnumerable< dynamic>> AppGetTintingMachineList(string territory);
        Task<TintiningMachineModel> AppCreateTiningMachine(TintiningMachineModel model);
        Task<TintiningMachineModel> AppUpdateTitningMachine(TintiningMachineModel model);
        #endregion

        #region Common
        Task<bool> DeleteTitiningMachine(int Id);
        Task<bool> IsTitiningMachineUpdatable(TintiningMachineModel model);
        Task<bool> IsExits(int Id);
        #endregion

        #region Portal
        Task<IPagedList<TintiningMachineModel>> GetTintingMachinePagingList(string territory, int index, int pageSize, string companyName);
        #endregion



    }
}
