﻿using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Tinting;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Tintining;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Tintining.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Tintining.Implementation
{
    public class TintiningService : ITintiningService
    {
        public readonly IRepository<TintiningMachine> _tiningMachineSvc;
        public readonly IRepository<DropdownType> _dropdownTypeSvc;
        public readonly IRepository<DropdownDetail> _dropdownDetailSvc;


        public TintiningService(
              IRepository<DropdownType> dropdownTypeSvc,
              IRepository<TintiningMachine> tiningMachineSvc,
              IRepository<DropdownDetail> dropdownDetailSvc
            )
        {
            _tiningMachineSvc = tiningMachineSvc;
            _dropdownTypeSvc = dropdownTypeSvc;
            _dropdownDetailSvc = dropdownDetailSvc;
        }
        public  async Task<bool> AppCreateTiningMachine(List<TintiningMachineModel> model)
        {
            var _tiningMachine = model.ToMap<TintiningMachineModel, TintiningMachine>();
              await _tiningMachineSvc.CreateListAsync(_tiningMachine);
            return true;

        }
        public async Task<List< TintiningMachineModel>> AppCreateTiningMachine(string employeeId,string territory)
        {
           var result= await Task.Run(()=> {
                var companys = _dropdownDetailSvc.Where(f => f.TypeId == 16);
                return companys
                    .GroupJoin(
                    _tiningMachineSvc.Where(f => f.TerritoryCd == territory && f.EmployeeId == employeeId), c => c.Id, t => t.CompanyId,
                     (c, t) => new { c, t })
                    .SelectMany(com => com.t.DefaultIfEmpty(), (c, t) => new TintiningMachineModel
                    {
                        Id = t.Id,
                        CompanyId = c.c.Id,
                        CompanyName = c.c.DropdownName,
                        Cont = t.Cont,
                        EmployeeId = t.EmployeeId,
                        No = t.No,
                        TerritoryCd = t.TerritoryCd

                    }).ToList();

            });
           
            return result;
          

        }

        public async Task<IEnumerable<dynamic>> AppGetTintingMachineList(string territory)
        {
            var param = new Dictionary<string, object>();
            param.Add("@trreitory", territory);
            return _tiningMachineSvc.DynamicListFromSql("Sp_GetTintingMachineByTerritory", param, true);
        }
        public async Task<IPagedList<TintiningMachineModel>> GetTintingMachinePagingList(int index,int pageSize,string search)
        {
           var result=await Task.Run(() =>
            {
                var tintings = _tiningMachineSvc.FindAll(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId);
                return tintings
                        .GroupJoin(
                        _dropdownDetailSvc.GetAll(), t => t.CompanyId, c => c.Id,
                         (t, c) => new { t, c })
                        .SelectMany(com => com.c.DefaultIfEmpty(), (t, c) => new TintiningMachineModel
                        {
                            Id = t.t.Id,
                            CompanyId = t.t.CompanyId,
                            CompanyName = c.DropdownName,
                            Cont = t.t.Cont,
                            EmployeeId = t.t.EmployeeId,
                            No = t.t.No,
                            TerritoryCd = t.t.TerritoryCd

                        }).ToList();
            });
            
            if (!string.IsNullOrEmpty(search))
                result = result.Search(search);
            return result.ToPagedList(index,pageSize);
            //var tintings = await _tiningMachineSvc.FindAllPagedAsync(f => f.TerritoryCd.Equals(territory) && f.EmployeeId == "0",index,pageSize);
           
            //decimal totalSumNo = _tiningMachineSvc.Where(f => f.TerritoryCd.Equals(territory)).Sum(f => f.No);
            //var result = from dd in _dropdownDetailSvc.GetAll()
            //               join dt in _dropdownTypeSvc.GetAll()
            //               on dd.TypeId equals dt.Id
            //               join t in tintings
            //               on dd.Id equals t.CompanyId
            //               select new TintiningMachineModel
            //               {
            //                   Id = t.Id,
            //                   CompanyId = t.CompanyId,
            //                   EmployeeId = t.EmployeeId,
            //                   CompanyName = dd.DropdownName,
            //                   No = t.No,
            //                   Cont = (float)(t.No / totalSumNo) * 100

            //               };
            //if (!string.IsNullOrEmpty(companyName))
            //    result = result.Where(f => f.CompanyName.Contains(companyName));
            //    return result.ToPagedList();
          //  var result= from tinting in _tiningMachineSvc.FindAll(f=>f.TerritoryCd==territory)
                        
            //var param = new Dictionary<string, object>();
            //param.Add("@trreitory", territory);
            // return _tiningMachineSvc.DynamicListFromSql("Sp_GetTintingMachineByTerritory", param,true);
           
        }

        public async Task<bool> AppUpdateTitningMachine(List<TintiningMachineModel> model)
        {

            var _tiningMachines = model.ToMap<TintiningMachineModel, TintiningMachine>();
            foreach (var tiningMachine in _tiningMachines)
            {
                tiningMachine.NoOfCorrection = 0;
                var find = await _tiningMachineSvc.FindAsync(f => f.CompanyId == tiningMachine.CompanyId
                                                               && f.TerritoryCd == tiningMachine.TerritoryCd && f.EmployeeId == tiningMachine.EmployeeId
                );
                if (find != null)
                    if (find.No > tiningMachine.No) tiningMachine.NoOfCorrection = find.NoOfCorrection + 1;
                    else tiningMachine.NoOfCorrection = find.NoOfCorrection;
                await _tiningMachineSvc.UpdateAsync(tiningMachine);
                return true;
            }
            return false;
            //_tiningMachine.NoOfCorrection = 0;
            //var find= await _tiningMachineSvc.FindAsync(
            //        f=> f.CompanyId == model.CompanyId
            //        && f.TerritoryCd == model.TerritoryCd
            //        && f.EmployeeId == model.EmployeeId
            //       );

            //if (find != null)
            //    if (find.No > _tiningMachine.No) _tiningMachine.NoOfCorrection = find.NoOfCorrection + 1;
            //    else _tiningMachine.NoOfCorrection = find.NoOfCorrection;


            //var result = await _tiningMachineSvc.UpdateAsync(_tiningMachine);
            //return result.ToMap<TintiningMachine, TintiningMachineModel>();

        }

        public async Task<bool> IsTitiningMachineUpdatable(TintiningMachineModel model)
        {
            var result = await _tiningMachineSvc.IsExistAsync(
                    f => f.TerritoryCd == model.TerritoryCd
                    && f.CompanyId == model.CompanyId
                    && f.EmployeeId == model.EmployeeId
                    && f.NoOfCorrection < 2
                    );
            return result;
                }
        

        public async Task<bool> DeleteTitiningMachine(int Id)
        {
            var result= await _tiningMachineSvc.DeleteAsync(f => f.Id == Id);
            if (result == 1) return true;
            else return false;
        }

        public async Task<bool> IsExits(int Id)=> await _tiningMachineSvc.IsExistAsync(f => f.Id == Id);
      
    }
}
