using Berger.Common.Constants;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.CollectionEntry.Implementation
{
    public class CollectionEntryService : ICollectionEntryService
    {
        private readonly IRepository<Payment> _payment;

        private readonly IRepository<CreditControlArea> _creditControlArea;
        private readonly IRepository<DropdownDetail> _dropdownDetail;
        private readonly ApplicationDbContext _context;

        public CollectionEntryService(
            IRepository<Payment> payment,
            IRepository<DropdownDetail> dropdownDetail,
        IRepository<CreditControlArea> creditControlArea,
        ApplicationDbContext context)
        {
            _payment = payment;
            _creditControlArea = creditControlArea;
            _dropdownDetail = dropdownDetail;
            _context = context;
        }
        public async Task<PaymentModel> CreateAsync(PaymentModel model)
        {
            var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.CreateAsync(mapModel);
            return result.ToMap<Payment, PaymentModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _payment.DeleteAsync(s => s.Id == id);

        public async Task<QueryResultModel<PaymentModel>> GetCollectionByType(CollectionReportSearchModel query)
        {
            var dealers = await (from dep in _context.Depots

                                 join uzam in _context.UserZoneAreaMappings on dep.Werks equals uzam.PlantId into uzamleftjoin
                                 from uzamaping in uzamleftjoin.DefaultIfEmpty()

                                 join u in _context.UserInfos on uzamaping.UserInfoId equals u.Id into uleftjoin
                                 from uinfo in uleftjoin.DefaultIfEmpty()

                                 join p in _context.Payments on uinfo.EmployeeId equals p.EmployeeId into pleftjoin
                                 from paym in pleftjoin.DefaultIfEmpty()

                                 join dct in _context.DropdownDetails on paym.CustomerTypeId equals dct.Id into dctleftjoin
                                 from dctinfo in dctleftjoin.DefaultIfEmpty()

                                 join dpm in _context.DropdownDetails on paym.PaymentMethodId equals dpm.Id into dpmleftjoin
                                 from dpminfo in dpmleftjoin.DefaultIfEmpty()

                                 join ca in _context.CreditControlAreas on paym.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                 from cainfo in caleftjoin.DefaultIfEmpty()
                                 join d in _context.DealerInfos on paym.DealerId equals d.Id.ToString() into dleftjoin
                                 from dinfo in dleftjoin.DefaultIfEmpty()

                                 join ter in _context.Territory on uzamaping.TerritoryId equals ter.Code into terleftjoin
                                 from terinfo in terleftjoin.DefaultIfEmpty()
                                 join zon in _context.Zone on uzamaping.ZoneId equals zon.Code into zonleftjoin
                                 from zonfo in zonleftjoin.DefaultIfEmpty()


                                 where (
                                   
                                   (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                   && (!query.Territories.Any() || query.Territories.Contains(uzamaping.TerritoryId))
                                   && (!query.Zones.Any() || query.Zones.Contains(uzamaping.ZoneId))
                                   && (!query.PaymentMethodId.HasValue || dpminfo.Id == query.PaymentMethodId.Value)
                                   && (!query.DealerId.HasValue || dinfo.Id == query.DealerId.Value)
                                   && (!query.Date.HasValue || paym.CollectionDate.Date == query.Date.Value.Date)
                                   && (!query.PaymentFromId.HasValue || dctinfo.Id == query.PaymentFromId)
                                   && (!string.IsNullOrEmpty(query.Depot) || uzamaping.PlantId == query.Depot)
                                 )
                                 select new
                                 {
                                     paym.CreditControlArea,
                                     paym.PaymentMethod,
                                     paym.CustomerType,
                                     paym.EmployeeId,
                                     paym.Id,
                                     uinfo.Email,
                                     paym.CollectionDate,
                                     customerType = dctinfo.DropdownName,
                                     paym.SapId,
                                     projectName = paym.Name,
                                     paym.Address,
                                     paymentMethod = dpminfo.DropdownName,
                                     creditControlArea = cainfo.Description,
                                     paym.BankName,
                                     paym.Number,
                                     paym.Amount,
                                     paym.ManualNumber,
                                     paym.Remarks,
                                     paym.Name,
                                     paym.MobileNumber,
                                     depotId = dep.Werks,
                                     depotName = dep.Name1,
                                     territoryName = terinfo.Name,
                                     zoneName = zonfo.Name,
                                     paym.DealerId,
                                     dinfo.CustomerNo
                                 }).ToListAsync();

            //var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControlArea).Where(f=>true).OrderByDescending(x => x.CollectionDate).
            var result = dealers.
            Select(s => new PaymentModel()
            {
                Id = s.Id,
                Amount = s.Amount,
                Address = s.Address,
                BankName = s.BankName,
                DealerId = s.DealerId,
                CreditControlAreaId = Convert.ToInt32(s.CreditControlArea.CreditControlAreaId),
                CreditControlAreaName = s.CreditControlArea.Description,
                ManualNumber = s.ManualNumber,
                MobileNumber = s.MobileNumber,
                Name = s.Name,
                Number = s.Number,
                CustomerTypeId = s.CustomerType.Id,
                PaymentMethodId = s.PaymentMethod.Id,
                PaymentMethodName = s.PaymentMethod.DropdownName,
                Remarks = s.Remarks,
                SapId = s.SapId,
                EmployeeId = s.EmployeeId,
                CollectionDate = s.CollectionDate.ToString("yyyy-MM-dd")



            }).OrderByDescending(p=>p.CollectionDate).ToList();

            var queryResult = new QueryResultModel<PaymentModel>();
            queryResult.Items = result;
            queryResult.TotalFilter = result.Count();
            queryResult.Total = result.Count();

            return queryResult;
        }

        public  async Task<IEnumerable<PaymentModel>> GetCollectionList()
        {
            try
            {
                var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControlArea);
                return result.Select(s => new PaymentModel()
                {
                    Id = s.Id,
                    Amount = s.Amount,
                    Address = s.Address,
                    BankName = s.BankName,
                    DealerId = s.DealerId,
                    CreditControlAreaId =Convert.ToInt32(s.CreditControlArea.CreditControlAreaId),
                    CreditControlAreaName = s.CreditControlArea.Description,
                    ManualNumber = s.ManualNumber,
                    MobileNumber = s.MobileNumber,
                    Name = s.Name,
                    Number = s.Number,
                    CustomerTypeId = s.CustomerType.Id,
                    PaymentMethodId = s.PaymentMethod.Id,
                    PaymentMethodName = s.PaymentMethod.DropdownName,
                    Remarks = s.Remarks,
                    SapId = s.SapId,
                    EmployeeId = s.EmployeeId,
                    CollectionDate=s.CollectionDate.ToString("yyyy-MM-dd")

                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex ;
            }

        }

        public async Task<IEnumerable<AppCollectionEntryModel>> GetAppCollectionListByCurrentUserAsync()
        {
            var employeeId = AppIdentity.AppUser.EmployeeId;
            var result = await _payment.GetAllIncludeAsync(x => 
                            new AppCollectionEntryModel 
                            { 
                                Id = x.Id,
                                CollectionDate = x.CollectionDate.ToString("yyyy-MM-dd"),
                                CustomerType = x.CustomerType.DropdownName,
                                PaymentMethod = x.PaymentMethod.DropdownName,
                                CreditControlArea = $"{x.CreditControlArea.Description} ({x.CreditControlAreaId})"
                            },
                            x => x.EmployeeId == employeeId,
                            //x => x.OrderByDescending(o => CustomConvertExtension.ObjectToDateTime(o.CollectionDate)),
                            x => x.OrderByDescending(o => o.CreatedTime),
                            x => x.Include(i => i.PaymentMethod).Include(i => i.CustomerType).Include(i => i.CreditControlArea),
                            true);

            return result;
        }

        public async Task<IEnumerable<CreditControlArea>> GetCreditControlAreaList()
        {
            var result = await _creditControlArea.GetAllAsync();
            return result;
        }

        public async Task<bool> IsExistAsync(int id) => await _payment.IsExistAsync(f => f.Id == id);

        public async Task<PaymentModel> UpdateAsync(Payment model)
        {
            //var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.UpdateAsync(model);
  
            return result.ToMap<Payment, PaymentModel>();
        }

        public async Task<PaymentModel> GetCollectionById(int Id)
        {
            var result =await _payment.FindByCondition(p => p.Id == Id).FirstOrDefaultAsync();
            var mapModel = result.ToMap<Payment, PaymentModel>();

            return mapModel;
        }
    }
}
