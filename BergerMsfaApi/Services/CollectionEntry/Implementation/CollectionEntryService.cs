using Berger.Data.MsfaEntity.CollectionEntry;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.CollectionEntry;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.CollectionEntry.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.CollectionEntry.Implementation
{
    public class CollectionEntryService : ICollectionEntryService
    {
        private readonly IRepository<Payment> _payment;
        public CollectionEntryService(IRepository<Payment> payment)
        {
            _payment = payment;
        }
        public async Task<PaymentModel> CreateAsync(PaymentModel model)
        {
            var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.CreateAsync(mapModel);
            return result.ToMap<Payment, PaymentModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _payment.DeleteAsync(s => s.Id == id);

        public async Task<IEnumerable<PaymentModel>> GetCollectionByType(string paymentFrom)
        {
            var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControllArea).Where(f=>f.PaymentFrom==paymentFrom);
            return result.Select(s => new PaymentModel()
            {
                Id = s.Id,
                Amount = s.Amount,
                Address = s.Address,
                BankName = s.BankName,
                Code = s.Code,
                CreditControllAreaId = s.CreditControllArea.Id,
                CreditControllAreaName = s.CreditControllArea.DropdownName,
                ManualNumber = s.ManualNumber,
                MobileNumber = s.MobileNumber,
                Name = s.Name,
                Number = s.Number,
                PaymentFrom = s.PaymentFrom,
                PaymentMethodId = s.PaymentMethod.Id,
                PaymentMethodName=s.PaymentMethod.DropdownName,
                Remarks = s.Remarks,
                SapId = s.SapId

            }).ToList();
        }

        public  async Task<IEnumerable<PaymentModel>> GetCollectionList()
        {
            try
            {
                var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControllArea);
                return result.Select(s => new PaymentModel()
                {
                    Id = s.Id,
                    Amount = s.Amount,
                    Address = s.Address,
                    BankName = s.BankName,
                    Code = s.Code,
                    CreditControllAreaId = s.CreditControllArea.Id,
                    CreditControllAreaName = s.CreditControllArea.DropdownName,
                    ManualNumber = s.ManualNumber,
                    MobileNumber = s.MobileNumber,
                    Name = s.Name,
                    Number = s.Number,
                    PaymentFrom = s.PaymentFrom,
                    PaymentMethodId = s.PaymentMethod.Id,
                    PaymentMethodName = s.PaymentMethod.DropdownName,
                    Remarks = s.Remarks,
                    SapId = s.SapId

                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex ;
            }
          
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
