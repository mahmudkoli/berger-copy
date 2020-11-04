using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Scheme.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Scheme.Implementation
{
    public class SchemeService : ISchemeService
    {
        private readonly IRepository<SchemeMaster> _schemeMasterSvc;
        private readonly IRepository<SchemeDetail> _schemeDetailSvc;
        public SchemeService(
            IRepository<SchemeMaster> schemeMasterSvc,
            IRepository<SchemeDetail> schemeDetailSvc
            )
        {
            _schemeMasterSvc = schemeMasterSvc;
            _schemeDetailSvc = schemeDetailSvc;
        }

        public async Task<bool> IsSchemeDetailAlreadyExist(int Id)
        {
            return await _schemeDetailSvc.IsExistAsync(f => f.Id == Id);
        }

        public async Task<bool> IsSchemeMasterAlreadyExist(int Id)
        {
            return await _schemeMasterSvc.IsExistAsync(f => f.Id == Id);
        }

        public async Task<SchemeDetailModel> PortalCreateSchemeDeatil(SchemeDetailModel model)
        {
            var schemeDetail = model.ToMap<SchemeDetailModel, SchemeDetail>();
            var result = await _schemeDetailSvc.CreateAsync(schemeDetail);
            return result.ToMap<SchemeDetail, SchemeDetailModel>();
        }

        public async Task<SchemeMasterModel> PortalCreateSchemeMasters(SchemeMasterModel model)
        {
            var schemeMaster = model.ToMap<SchemeMasterModel, SchemeMaster>();
            var result = await _schemeMasterSvc.CreateAsync(schemeMaster);
            return result.ToMap<SchemeMaster, SchemeMasterModel>();
        }

        public async Task<int> PortalDeleteSchemeDetail(int Id)
        {

            return await _schemeDetailSvc.DeleteAsync(f => f.SchemeMasterId == Id);

        }

        public async Task<int> PortalDeleteSchemeMasters(int Id)
        {

            var find = await _schemeDetailSvc.AnyAsync(f => f.Id == Id);
            if (find) await _schemeDetailSvc.DeleteAsync(f => f.SchemeMasterId == Id);
            return await _schemeMasterSvc.DeleteAsync(f => f.Id == Id);
        }

        public async Task<IEnumerable<SchemeDetailModel>> PortalGetcShemeDetailWithMaster()
        {

            var result = new List<SchemeDetailModel>();
            var list =  _schemeMasterSvc.GetAllInclude(f=>f.SchemeDetail);
            if (list.Count() > 0)
            {
                 result = list
                    .SelectMany(s => s.SchemeDetail.
                     Select(d => new SchemeDetailModel
                     {
                         Id = d.Id,
                         SchemeName = s.SchemeName,
                         GlobalCondition = s.Condition,
                         Code = d.Code,
                         Slab = d.Slab,
                         Condition = d.Condition,
                         Item = d.Item,
                         Date = d.Date,
                         TargetVolume = d.TargetVolume,
                         Benefit = d.Benefit
                     })).ToList();
            }
            return result;
        }

        public async Task<IEnumerable<SchemeDetailModel>> PortalGetSchemeDelails()
        {
            var result = await _schemeDetailSvc.GetAllAsync();
            return result.ToMap<SchemeDetail, SchemeDetailModel>();
        }

        public async Task<SchemeDetailModel> PortalGetSchemeDetailById(int Id)
        {
            var result = await _schemeDetailSvc.FindAsync(f => f.Id == Id);
            return result.ToMap<SchemeDetail, SchemeDetailModel>();
        }

        public async Task<IEnumerable<SchemeMasterModel>> PortalGetSchemeMasters()
        {
            var result = await _schemeMasterSvc.GetAllAsync();
            return result.ToMap<SchemeMaster, SchemeMasterModel>();

        }

        public async Task<SchemeMasterModel> PortalGetSchemeMastersById(int Id)
        {
            var result = await _schemeMasterSvc.FindAsync(f => f.Id == Id);
            return result.ToMap<SchemeMaster, SchemeMasterModel>();
        }

        public async Task<SchemeDetailModel> PortalUpdateSchemeDetail(SchemeDetailModel model)
        {
            var schemeDetail = model.ToMap<SchemeDetailModel, SchemeDetail>();
            var result = await _schemeDetailSvc.UpdateAsync(schemeDetail);
            return result.ToMap<SchemeDetail, SchemeDetailModel>();
        }

        public async Task<SchemeMasterModel> PortalUpdateSchemeMasters(SchemeMasterModel model)
        {
            var schemeMaster = model.ToMap<SchemeMasterModel, SchemeMaster>();
            var result = await _schemeMasterSvc.UpdateAsync(schemeMaster);
            return result.ToMap<SchemeMaster, SchemeMasterModel>();
        }
    }
}
