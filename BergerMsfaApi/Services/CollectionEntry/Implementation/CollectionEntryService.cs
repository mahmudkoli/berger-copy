using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.CollectionEntry;
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
        public CollectionEntryService(
            IRepository<Payment> payment,
            IRepository<DropdownDetail> dropdownDetail,
        IRepository<CreditControlArea> creditControlArea)
        {
            _payment = payment;
            _creditControlArea = creditControlArea;
            _dropdownDetail = dropdownDetail;
        }
        public async Task<PaymentModel> CreateAsync(PaymentModel model)
        {
            var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.CreateAsync(mapModel);
            return result.ToMap<Payment, PaymentModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _payment.DeleteAsync(s => s.Id == id);

        public async Task<IEnumerable<PaymentModel>> GetCollectionByType(int CustomerTypeId)
        {
            
            var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControlArea).Where(f=>f.CustomerTypeId==CustomerTypeId).OrderByDescending(x => x.CollectionDate);
            return result.Select(s => new PaymentModel()
            {
                Id = s.Id,
                Amount = s.Amount,
                Address = s.Address,
                BankName = s.BankName,
                Code = s.Code,
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



            }).ToList();
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
                    Code = s.Code,
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

        public async Task<PaymentModel> UpdateAsync(PaymentModel model)
        {
            var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.UpdateAsync(mapModel);
  
            return result.ToMap<Payment, PaymentModel>();
        }
    }
}
