using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Tinting;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Tintining;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Tintining.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Tintining.Implementation
{
    public class TintiningService : ITintiningService
    {
        public readonly IRepository<TintiningMachine> _tiningMachineSvc;


        public TintiningService(IRepository<TintiningMachine> tiningMachineSvc)
        {
            _tiningMachineSvc = tiningMachineSvc;
        }
        public  async Task<TintiningMachineModel> AppCreateTiningMachine(TintiningMachineModel model)
        {
            var _tiningMachine = model.ToMap<TintiningMachineModel, TintiningMachine>();
            var result = await _tiningMachineSvc.CreateAsync(_tiningMachine);
            return result.ToMap<TintiningMachine, TintiningMachineModel>();

        }

        public async Task<IEnumerable<TintiningMachineModel>> AppGetTintingMachineList(string territory)
        {
            var result = await _tiningMachineSvc.FindAllAsync(f => f.TerritoryCd == territory);
            return result.ToMap<TintiningMachine, TintiningMachineModel>();
        }
        public async Task<IEnumerable<dynamic>> GetTintingMachineList(string territory)


        {
            var param = new Dictionary<string, object>();
            param.Add("@trreitory", territory);
             return _tiningMachineSvc.DynamicListFromSql("Sp_GetTintingMachineByTerritory", param,true);
           
        }

        public async Task<TintiningMachineModel> AppUpdateTitningMachine(TintiningMachineModel model)
        {
   
            var _tiningMachine = model.ToMap<TintiningMachineModel, TintiningMachine>();
            _tiningMachine.NoOfCorrection = 0;
            var find= await _tiningMachineSvc.FindAsync(
                    f=> f.CompanyId == model.CompanyId
                    && f.TerritoryCd == model.TerritoryCd
                    && f.EmployeeId == model.EmployeeId
                   );

            if (find != null)
                if (find.No > _tiningMachine.No) _tiningMachine.NoOfCorrection =find.NoOfCorrection+1;

            var result = await _tiningMachineSvc.UpdateAsync(_tiningMachine);
            return result.ToMap<TintiningMachine, TintiningMachineModel>();

        }

        public async Task<bool> IsTitiningMachineUpdatable(TintiningMachineModel model)
            => await _tiningMachineSvc.IsExistAsync(
                    f => f.NoOfCorrection < 2
                    && f.TerritoryCd==model.TerritoryCd
                    && f.CompanyId==model.CompanyId
                    && f.EmployeeId==model.EmployeeId 
                    );
        

        public async Task<bool> DeleteTitiningMachine(int Id)
        {
            var result= await _tiningMachineSvc.DeleteAsync(f => f.Id == Id);
            if (result == 1) return true;
            else return false;
        }

        public async Task<bool> IsExits(int Id)=> await _tiningMachineSvc.IsExistAsync(f => f.Id == Id);
      
    }
}
