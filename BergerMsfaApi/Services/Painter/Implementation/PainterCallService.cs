using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Implementation
{
    public class PaintCallService : IPaintCallService
    {
        private readonly IRepository<PainterCall> _painterCallSvc;
        private readonly IRepository<PainterCompanyMTDValue> _painterCompanyMtvSvc;
        private readonly IRepository<DropdownDetail> _dropdownDetailSvc;

        public PaintCallService(
               IRepository<PainterCall> painterCallSvc,
               IRepository<PainterCompanyMTDValue> painterCompanyMtvSvc,
               IRepository<DropdownDetail> dropdownDetailSvc
            )
        {
            _painterCallSvc = painterCallSvc;
            _painterCompanyMtvSvc = painterCompanyMtvSvc;
            _dropdownDetailSvc=dropdownDetailSvc;

    }

        public async Task<PainterCallModel> CreatePainterCallAsync(PainterCallModel model)
        {

            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>();

            }).CreateMapper();

            var painterCall = _mapper.Map<PainterCall>(model);
            var result = await _painterCallSvc.CreateAsync(painterCall);
            return _mapper.Map<PainterCallModel>(result);
        }

        public async Task<PainterCallModel> UpdatePainterCallAsync(PainterCallModel model)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();

            }).CreateMapper();

            var _painterCall = await _painterCallSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.PainterCompanyMTDValue);
            _painterCall = _mapper.Map<PainterCall>(model);
            var updatePainterCall = await _painterCallSvc.UpdateAsync(_painterCall);

            return _mapper.Map<PainterCallModel>(updatePainterCall);
        }
        public async Task<int> DeletePainterCallByIdlAsync(int PainterId)
        {

            return await _painterCallSvc.DeleteAsync(f => f.PainterId == PainterId);
        }
        
        public async Task<PainterCallModel> GetPainterByIdAsync(int PainterId)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();
              

            }).CreateMapper();
            var painterCall = await _painterCallSvc.FindIncludeAsync(f => f.PainterId == PainterId, f => f.PainterCompanyMTDValue);
            return _mapper.Map<PainterCallModel>(painterCall);

        }

        public async Task<IEnumerable<PainterCallModel>> GetPainterCallListAsync()
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();
            

            }).CreateMapper();

            var result = await _painterCallSvc.GetAllIncludeAsync(
                s => s,
                f => f.EmployeeId == AppIdentity.AppUser.EmployeeId,
                null,
                f => f.Include(i => i.PainterCompanyMTDValue), 
                true);

            var painterModel = _mapper.Map<List<PainterCallModel>>(result);
            return painterModel;

        }

        public async Task<bool> IsExistAsync(int Id) => await _painterCallSvc.IsExistAsync(f => f.Id == Id);

        public async Task<IEnumerable<PainterCallModel>> AppGetPainterCallListAsync(string emplyeeId)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();
            }).CreateMapper();


            var result = await _painterCallSvc.GetAllIncludeAsync(
               s => s,
               f => f.EmployeeId ==emplyeeId,
               null,
               f => f.Include(i => i.PainterCompanyMTDValue),
               true);

            var painterModel = _mapper.Map<List<PainterCallModel>>(result);
            return painterModel;
        }

        public async Task<List<PainterCallModel>> AppGetPainterByPainterIdAsync(string employeeId,int PainterId)
        {
            var companys = _dropdownDetailSvc.GetAllInclude(f => f.DropdownType).Where(f => f.DropdownType.TypeCode == DynamicTypeCode.PaintUsage);
            var result = await _painterCallSvc.GetAllIncludeAsync(
                                        s => s, f => f.EmployeeId == employeeId && f.PainterId == PainterId,
                                        null, f => f.Include(i => i.PainterCompanyMTDValue),
                                        true);

            return result.Select(_painterCall => new PainterCallModel
            {
                Id = _painterCall.Id,
                HasAppUsage = _painterCall.HasAppUsage,
                Comment = _painterCall.Comment,
                HasDbblIssue = _painterCall.HasDbblIssue,
                HasNewProBriefing = _painterCall.HasNewProBriefing,
                HasPremiumProtBriefing = _painterCall.HasPremiumProtBriefing,
                HasSchemeComnunaction = _painterCall.HasSchemeComnunaction,
                HasUsageEftTools = _painterCall.HasUsageEftTools,
                PainterId = _painterCall.PainterId,
                WorkInHandNumber = _painterCall.WorkInHandNumber,
                PainterCompanyMTDValue = (from c in companys
                                          join m in _painterCompanyMtvSvc.GetAll()
                                          on new { a = c.Id, b = _painterCall.Id } equals new { a = m.CompanyId, b = m.PainterCallId } into comLeftJoin
                                          from coms in comLeftJoin.DefaultIfEmpty()
                                          select new PainterCompanyMTDValueModel
                                          {
                                              CompanyId = c.Id,
                                              CompanyName = c.DropdownName,
                                              Value = coms.Value,
                                              CountInPercent = coms != null ? coms.CountInPercent : 0,
                                              CumelativeInPercent = coms != null ? coms.CountInPercent : 0

                                          }).ToList()

            }).ToList();
        }
        public async Task<PainterCallModel> AppGetPainterByIdAsync(int Id)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();

            }).CreateMapper();

            var result = await _painterCallSvc.FindIncludeAsync(f => f.Id == Id, f => f.PainterCompanyMTDValue);
            return _mapper.Map<PainterCallModel>(result);
        }
        public async Task<PainterCallModel> AppCreatePainterCallAsync(string employeeId,PainterCallModel model)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();

            }).CreateMapper();

            var painterCall = _mapper.Map<PainterCall>(model);
            painterCall.EmployeeId = employeeId;
            var result = await _painterCallSvc.CreateAsync(painterCall);
            return _mapper.Map<PainterCallModel>(result);
        }
        public async Task<PainterCallModel> AppCreatePainterCallAsync(int PainterId)
        {
            var result = _dropdownDetailSvc.GetAllInclude(f => f.DropdownType).Where(f => f.DropdownType.TypeCode == DynamicTypeCode.PaintUsage);
            if (await _painterCallSvc.AnyAsync(p => p.PainterId == PainterId))
            {
                var _painterCall = _painterCallSvc
                    .Where(f => f.PainterId == PainterId)
                    .OrderByDescending(f=>f.Id).FirstOrDefault();

                return new PainterCallModel
                {
                    //Id = _painterCall.Id,
                    HasAppUsage = _painterCall.HasAppUsage,
                    Comment = _painterCall.Comment,
                    HasDbblIssue = _painterCall.HasDbblIssue,
                    HasNewProBriefing = _painterCall.HasNewProBriefing,
                    HasPremiumProtBriefing = _painterCall.HasPremiumProtBriefing,
                    HasSchemeComnunaction = _painterCall.HasSchemeComnunaction,
                    HasUsageEftTools = _painterCall.HasUsageEftTools,
                    PainterId = _painterCall.PainterId,
                    WorkInHandNumber = _painterCall.WorkInHandNumber,
                    PainterCompanyMTDValue = (from c in result
                                              join m in _painterCompanyMtvSvc.GetAll()
                                              on new { a = c.Id, b =_painterCall.Id  } equals new { a = m.CompanyId, b = m.PainterCallId } into comLeftJoin
                                              from coms in comLeftJoin.DefaultIfEmpty()
                                              select new PainterCompanyMTDValueModel
                                              {
                                                  CompanyId = c.Id,
                                                  CompanyName = c.DropdownName,
                                                  Value = coms.Value,
                                                  CountInPercent = coms != null ? coms.CountInPercent : 0,
                                                  CumelativeInPercent = coms != null ? coms.CountInPercent : 0

                                              }).ToList()

                };
            }



            //if (_painterCall != null)
            //    return new PainterCallModel
            //    {
            //        Id = _painterCall.Id,
            //        HasAppUsage = _painterCall.HasAppUsage,
            //        Comment = _painterCall.Comment,
            //        HasDbblIssue = _painterCall.HasDbblIssue,
            //        HasNewProBriefing = _painterCall.HasNewProBriefing,
            //        HasPremiumProtBriefing = _painterCall.HasPremiumProtBriefing,
            //        HasSchemeComnunaction = _painterCall.HasSchemeComnunaction,
            //        HasUsageEftTools = _painterCall.HasUsageEftTools,
            //        PainterId = _painterCall.PainterId,
            //        WorkInHandNumber = _painterCall.WorkInHandNumber,
            //        PainterCompanyMTDValue = (from c in result
            //                                  join m in _painterCompanyMtvSvc.GetAll()
            //                                  on new { a = c.Id, b = painterCallId } equals new { a = m.CompanyId, b = m.PainterCallId } into comLeftJoin
            //                                  from coms in comLeftJoin.DefaultIfEmpty()
            //                                  select new PainterCompanyMTDValueModel
            //                                  {  
            //                                      CompanyId = c.Id,
            //                                      CompanyName = c.DropdownName,
            //                                      Value = coms.Value,
            //                                      CountInPercent = coms != null ? coms.CountInPercent : 0,
            //                                      CumelativeInPercent = coms != null ? coms.CountInPercent : 0

            //                                  }).ToList()
            //    };
            else return new PainterCallModel
            {
                PainterId = PainterId,
                PainterCompanyMTDValue = (from c in result
                                          join m in _painterCompanyMtvSvc.GetAll()
                                          on new { a = c.Id } equals new { a = m.CompanyId } into comLeftJoin
                                          from coms in comLeftJoin.DefaultIfEmpty()
                                          select new PainterCompanyMTDValueModel
                                          {
                                              CompanyId = c.Id,
                                              CompanyName = c.DropdownName,
                                              Value = coms.Value,
                                              CountInPercent = coms != null ? coms.CountInPercent : 0,
                                              CumelativeInPercent = coms != null ? coms.CountInPercent : 0

                                          }).ToList()
            };
            

        }

        public async Task<PainterCallModel> AppUpdatePainterCallAsync(string employeeId,PainterCallModel model)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ReverseMap();

            }).CreateMapper();

            var _painterCall = await _painterCallSvc.FindIncludeAsync(f => f.Id == model.Id && f.EmployeeId==employeeId, f => f.PainterCompanyMTDValue);
            _painterCall = _mapper.Map<PainterCall>(model);
            _painterCall.EmployeeId = employeeId;
            var updatePainterCall = await _painterCallSvc.UpdateAsync(_painterCall);
            return _mapper.Map<PainterCallModel>(updatePainterCall);
        }

        
    }
}
