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
        Task<List<TintiningMachineModel>> AppCreateTiningMachine(string employeeId,string territory);
        Task<IEnumerable< dynamic>> AppGetTintingMachineList(string territory);
        Task<bool> AppCreateTiningMachine(List<TintiningMachineModel> model);
        Task<bool> AppUpdateTitningMachine(List<TintiningMachineModel> model);
        #endregion

        #region Common
        Task<bool> DeleteTitiningMachine(int Id);
        Task<bool> IsTitiningMachineUpdatable(TintiningMachineModel model);
        Task<bool> IsExits(int Id);
        #endregion

        #region Portal
        Task<IPagedList<TintiningMachineModel>> GetTintingMachinePagingList(int index, int pageSize, string search);
        #endregion



    }
}
