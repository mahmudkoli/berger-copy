using AutoMapper;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Implementation
{
    public class PaintCallService : IPaintCallService
    {
        private readonly IRepository<PainterCall> _painterCallSvc;
        private readonly IRepository<PainterCompanyMTDValue> _painterCompanyMtvSvc;

        public PaintCallService(
               IRepository<PainterCall> painterCallSvc,
               IRepository<PainterCompanyMTDValue> painterCompanyMtvSvc
            )
        {
            _painterCallSvc = painterCallSvc;
            _painterCompanyMtvSvc = painterCompanyMtvSvc;

        }

        public async Task<PainterCallModel> CreatePainterCallAsync(PainterCallModel model)
        {

            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

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
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

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
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();
            var painterCall = await _painterCallSvc.FindIncludeAsync(f => f.PainterId == PainterId, f => f.PainterCompanyMTDValue);
            return _mapper.Map<PainterCallModel>(painterCall);

        }

        public async Task<IEnumerable<PainterCallModel>> GetPainterCallListAsync()
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();
            var result = await _painterCallSvc.GetAllInclude(f => f.PainterCompanyMTDValue).ToListAsync();
            var painterModel = _mapper.Map<List<PainterCallModel>>(result);
            return painterModel;

        }

        public async Task<bool> IsExistAsync(int Id) => await _painterCallSvc.IsExistAsync(f => f.Id == Id);

        public async Task<IEnumerable<PainterCallModel>> AppGetPainterCallListAsync()
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();
            var result = await _painterCallSvc.GetAllInclude(f => f.PainterCompanyMTDValue).ToListAsync();
            var painterModel = _mapper.Map<List<PainterCallModel>>(result);
            return painterModel;
        }

        public async Task<PainterCallModel> AppGetPainterByPainterIdAsync(int PainterId)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();
       
            var result = await _painterCallSvc.FindIncludeAsync(f=>f.PainterId==PainterId,f=>f.PainterCompanyMTDValue);
            return _mapper.Map<PainterCallModel>(result);
        }
        public async Task<PainterCallModel> AppGetPainterByIdAsync(int Id)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();

            var result = await _painterCallSvc.FindIncludeAsync(f => f.Id == Id, f => f.PainterCompanyMTDValue);
            return _mapper.Map<PainterCallModel>(result);
        }
        public async Task<PainterCallModel> AppCreatePainterCallAsync(PainterCallModel model)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();

            var painterCall = _mapper.Map<PainterCall>(model);
            var result = await _painterCallSvc.CreateAsync(painterCall);
            return _mapper.Map<PainterCallModel>(result);
        }

        public async Task<PainterCallModel> AppUpdatePainterCallAsync(PainterCallModel model)
        {
            var _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<PainterCompanyMTDValueModel, PainterCompanyMTDValue>().ReverseMap();
                config.CreateMap<PainterCall, PainterCallModel>().ForMember(dest => dest.PainterCompanyMTDValueModel, src => src.MapFrom(m => m.PainterCompanyMTDValue)).ReverseMap();

            }).CreateMapper();

            var _painterCall = await _painterCallSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.PainterCompanyMTDValue);
            _painterCall = _mapper.Map<PainterCall>(model);
            var updatePainterCall = await _painterCallSvc.UpdateAsync(_painterCall);
            return _mapper.Map<PainterCallModel>(updatePainterCall);
        }
    }
}
