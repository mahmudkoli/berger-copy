using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Setup.Implementation
{
    public class DropdownService : IDropdownService
    {
        private readonly IRepository<DropdownDetail> _dropdownDetail;
        private readonly IRepository<DropdownType> _dropdownType;
        private readonly IRepository<PainterCompanyMTDValue> _painterCompanyMTDsvc;

        public DropdownService(
            IRepository<DropdownDetail> dropdownDetail,
            IRepository<DropdownType> dropdownType,
            IRepository<PainterCompanyMTDValue> painterCompanyMTDsvc
            )
        {
            _dropdownDetail = dropdownDetail;
            _dropdownType = dropdownType;
            _painterCompanyMTDsvc = painterCompanyMTDsvc;
        }

        public async Task<IEnumerable<DropdownModel>> GetDropdownList()
        {
            var result = _dropdownDetail.GetAllInclude(f => f.DropdownType);

            return result.Select(s => new DropdownModel()
            {
                Id = s.Id,
                TypeId = s.TypeId,
                TypeCode = s.DropdownType.TypeCode,
                TypeName = s.DropdownType.TypeName,
                DropdownName = s.DropdownName,
                Description = s.Description,
                Sequence = s.Sequence
            }).ToList();
        }

        public async  Task<IPagedList<DropdownModel>> GetDropdownListPaging(int index, int pageSize)
        {
            IPagedList<DropdownDetail> result = await _dropdownDetail.GetAllPagedAsync(index, pageSize);

            return new PagedList<DropdownModel>(result, result.Select(s => new DropdownModel
            {
                Id = s.Id,
                TypeId = s.TypeId,
                TypeCode = _dropdownType.Find(f => f.Id == s.TypeId).TypeCode,
                TypeName = _dropdownType.Find(f => f.Id == s.TypeId).TypeName,
                DropdownName = s.DropdownName,
                Description = s.Description,
                Sequence = s.Sequence

            }).ToPagedList());
            //return result.Select(s => new DropdownModel
            //{
            //    Id = s.Id,
            //    TypeId = s.TypeId,
            //    TypeCode = _dropdownType.Find(f => f.Id == s.TypeId).TypeCode,
            //    TypeName = _dropdownType.Find(f => f.Id == s.TypeId).TypeName,
            //    DropdownName = s.DropdownName,
            //    Description = s.Description,
            //    Sequence = s.Sequence

            //}).ToPagedList();
        }

        public async Task<DropdownModel> GetDropdownById(int id)
        {
            var result = await _dropdownDetail.FindIncludeAsync(f => f.Id == id, f => f.DropdownType);

            return new DropdownModel
            {
                Id = result.Id,
                TypeId = result.TypeId,
                TypeCode = result.DropdownType.TypeCode,
                TypeName = result.DropdownType.TypeName,
                DropdownName = result.DropdownName,
                Description = result.Description,
                Sequence = result.Sequence
            };
        }

        public async Task<int> GetLastSquence(int id, int typeId)
        {
            var result = _dropdownDetail.Find(f => f.Id == id && f.TypeId == typeId);
            if (result != null) return result.Sequence;
            else
                return await _dropdownDetail.AnyAsync(f => f.TypeId == typeId) ? _dropdownDetail.Where(f => f.TypeId == typeId).OrderByDescending(f => f.Sequence).FirstOrDefault().Sequence + 1 : 1;
        }

        public async Task<IEnumerable<DropdownType>> GetDropdownTypeList() => await _dropdownType.GetAllAsync();

        public async Task<DropdownModel> CreateAsync(DropdownModel model)
        {
            var example = model.ToMap<DropdownModel, DropdownDetail>();
            var result = await _dropdownDetail.CreateAsync(example);

            return result.ToMap<DropdownDetail, DropdownModel>();
        }

        public async Task<DropdownModel> UpdateAsync(DropdownModel model)
        {
            var data = model.ToMap<DropdownModel, DropdownDetail>();
            var result = await _dropdownDetail.UpdateAsync(data);
            return result.ToMap<DropdownDetail, DropdownModel>();
        }

        public async Task<int> DeleteAsync(int id) => await _dropdownDetail.DeleteAsync(s => s.Id == id);

        public async Task<bool> IsExistAsync(int id) =>  await _dropdownDetail.IsExistAsync(f => f.Id == id);

        public async Task<IEnumerable<DropdownModel>> GetDropdownByTypeCd(string typeCode)
        {
            var result = _dropdownDetail.GetAllInclude(f => f.DropdownType).Where(f => f.DropdownType.TypeCode == typeCode);

            return result.Select(s => new DropdownModel()
            {
                Id = s.Id,
                TypeId = s.TypeId,
                TypeCode = s.DropdownType.TypeCode,
                TypeName = s.DropdownType.TypeName,
                DropdownName = s.DropdownName,
                Description = s.Description,
                Sequence = s.Sequence
            }).ToList();
        }

        public async Task<IEnumerable<DropdownModel>> GetDropdownByTypeCd(IList<string> typeCodes)
        {
            var result = _dropdownDetail.GetAllInclude(f => f.DropdownType).Where(f => typeCodes.Any(t => t == f.DropdownType.TypeCode));

            return result.Select(s => new DropdownModel()
            {
                Id = s.Id,
                TypeId = s.TypeId,
                TypeCode = s.DropdownType.TypeCode,
                TypeName = s.DropdownType.TypeName,
                DropdownName = s.DropdownName,
                Description = s.Description,
                Sequence = s.Sequence
            }).ToList();
        }

        public async Task<IEnumerable<DropdownModel>> GetDropdownByTypeId(int typeId)
        {
            var result = _dropdownDetail.GetAllInclude(f => f.DropdownType).Where(f=>f.TypeId==typeId);

            return result.Select(s => new DropdownModel()
            {
                Id = s.Id,
                TypeId = s.TypeId,
                TypeCode = s.DropdownType.TypeCode,
                TypeName = s.DropdownType.TypeName,
                DropdownName = s.DropdownName,
                Description = s.Description,
                Sequence = s.Sequence
            }).ToList();
        }

        public async Task<IEnumerable<PainterCompanyMTDValueModel>> GetCompanyList(int PainterCallId)
        {
            var result = from dt in _dropdownType.GetAll()
                         join dd in _dropdownDetail.GetAll()
                         on dt.Id equals dd.TypeId
                         where dt.TypeCode == DynamicTypeCode.PaintUsageCompany
                         select dd;
            //TypeId will change
           // var result = _dropdownDetail.GetAllInclude(f => f.DropdownType).Where(f => f.TypeId ==16);
            var company = (from c in result
                          join m in _painterCompanyMTDsvc.GetAll()
                          on new { a=c.Id,b= PainterCallId } equals new { a=m.CompanyId,b=m.PainterCallId} into comLeftJoin
                          from coms in comLeftJoin.DefaultIfEmpty()
                          select new PainterCompanyMTDValueModel
                          { 
                              CompanyId=c.Id,
                              CompanyName=c.DropdownName,
                              Value=coms.Value,
                              CountInPercent=coms!=null?coms.CountInPercent:0,
                              CumelativeInPercent= coms != null ? coms.CountInPercent:0
                              

                          }).ToList();

            return company;
          
        }
    }
}
