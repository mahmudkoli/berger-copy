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
            var dealers = await (from p in _context.Payments
                                 join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                 from uinfo in uleftjoin.DefaultIfEmpty()
                                 join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                 from dctinfo in dctleftjoin.DefaultIfEmpty()
                                 join dpm in _context.DropdownDetails on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                                 from dpminfo in dpmleftjoin.DefaultIfEmpty()
                                 join ca in _context.CreditControlAreas on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                 from cainfo in caleftjoin.DefaultIfEmpty()
                                 join d in _context.DealerInfos on p.DealerId equals d.Id.ToString() into dleftjoin
                                 from dinfo in dleftjoin.DefaultIfEmpty()
                                 join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                 from depinfo in depleftjoin.DefaultIfEmpty()
                                 where (
                                   
                                   (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                   && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                   && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                   && (!query.PaymentMethodId.HasValue || dpminfo.Id == query.PaymentMethodId.Value)
                                   && (!query.DealerId.HasValue || dinfo.Id == query.DealerId.Value)
                                   && (!query.Date.HasValue || p.CollectionDate.Date == query.Date.Value.Date)
                                   && (!query.PaymentFromId.HasValue || dctinfo.Id == query.PaymentFromId)
                                   && (!string.IsNullOrEmpty(query.Depot) || p.Depot == query.Depot)
                                 )
                                 orderby p.CreatedTime descending
                                 select new
                                 {
                                     p.CreditControlArea,
                                     p.PaymentMethod,
                                     p.CustomerType,
                                     p.EmployeeId,
                                     p.Id,
                                     uinfo.Email,
                                     p.CollectionDate,
                                     customerType = dctinfo.DropdownName,
                                     p.SapId,
                                     projectName = p.Name,
                                     p.Address,
                                     paymentMethod = dpminfo.DropdownName,
                                     creditControlArea = cainfo.Description,
                                     p.BankName,
                                     p.Number,
                                     p.Amount,
                                     p.ManualNumber,
                                     p.Remarks,
                                     p.Name,
                                     p.MobileNumber,
                                     depotId = depinfo.Werks,
                                     depotName = depinfo.Name1,
                                     territoryName = p.Territory,
                                     zoneName = p.Zone,
                                     p.DealerId,
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
