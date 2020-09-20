using BergerMsfaApi.Domain.CollectionEntry;
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
            var result = _payment.GetAllInclude(f => f.PaymentMethod, f => f.CreditControlArea).Where(f=>f.PaymentForm==paymentFrom);
            return result.Select(s => new PaymentModel()
            {
                Id = s.Id,
                Amount = s.Amount,
                Address = s.Address,
                BankName = s.BankName,
                Code = s.Code,
                CreditControllAreaId = s.CreditControlArea.Id,
                CreditControllAreaName = s.CreditControlArea.DropdownName,
                ManualNumber = s.ManualNumber,
                MobileNumber = s.MobileNumber,
                Name = s.Name,
                Number = s.Number,
                PaymentForm = s.PaymentForm,
                PaymentMethodId = s.PaymentMethod.Id,
                PaymentMethodName=s.PaymentMethod.DropdownName,
                Remarks = s.Remarks,
                SAPID = s.SAPID

            }).ToList();
        }

        public  async Task<IEnumerable<PaymentModel>> GetCollectionList()
        {
            var result = _payment.GetAllInclude(f => f.PaymentMethod,f=>f.CreditControlArea);
           return result.Select(s => new PaymentModel()
            {
                Id = s.Id,
               Amount = s.Amount,
               Address=s.Address,
               BankName=s.BankName,
               Code=s.Code,
               CreditControllAreaId=s.CreditControlArea.Id,
               CreditControllAreaName = s.CreditControlArea.DropdownName,
               ManualNumber =s.ManualNumber,
               MobileNumber=s.MobileNumber,
               Name=s.Name,
               Number=s.Number,
               PaymentForm=s.PaymentForm,
               PaymentMethodId=s.PaymentMethod.Id,
               PaymentMethodName = s.PaymentMethod.DropdownName,
               Remarks =s.Remarks,
               SAPID=s.SAPID
                
            }).ToList();
        }

        public async Task<bool> IsExistAsync(PaymentModel model) => model.Id > 0 ? await _payment.IsExistAsync(f => f.Id == model.Id) : false;

        public async Task<PaymentModel> UpdateAsync(PaymentModel model)
        {
            var mapModel = model.ToMap<PaymentModel, Payment>();
            var result = await _payment.UpdateAsync(mapModel);
            return result.ToMap<Payment, PaymentModel>();
        }
    }
}
