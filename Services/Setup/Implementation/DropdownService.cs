using BergerMsfaApi.Domain.Setup;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Setup.Implementation
{
    public class DropdownService : IDropdownService
    {
        private readonly IRepository<DropdownDetail> _dropdownDetail;
        private readonly IRepository<DropdownType> _dropdownType;

        public DropdownService(
            IRepository<DropdownDetail> dropdownDetail,
            IRepository<DropdownType> dropdownType
            )
        {
            _dropdownDetail = dropdownDetail;
            _dropdownType = dropdownType;
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
            var example = model.ToMap<DropdownModel, DropdownDetail>();
            var result = await _dropdownDetail.UpdateAsync(example);
            return result.ToMap<DropdownDetail, DropdownModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _dropdownDetail.DeleteAsync(s => s.Id == id);

        public async Task<bool> IsExistAsync(DropdownModel model) => model.Id > 0 ? await _dropdownDetail.IsExistAsync(f => f.Id == model.Id) : false;

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
    }
}
