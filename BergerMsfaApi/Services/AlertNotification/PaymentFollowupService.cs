using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
    public class PaymentFollowupService : IPaymentFollowupService
    {
        private readonly IRepository<PaymentFollowupNotification> _repository;
        public PaymentFollowupService(IRepository<PaymentFollowupNotification> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PaymentFollowupNotification>> GetToayPaymentFollowup(IList<string> customer)
        {
            var res = _repository.Where(p => p.NotificationDate.Date == DateTime.Now.Date
                                           && customer.Contains(p.CustomarNo)
                                           && (p.IsRprsPayment || p.IsFastPayCarryPayment)
                                           ).ToList();
            return res;
        }

        public async Task<bool> SavePaymentFollowup(IList<PaymentFollowupNotification> paymentFollowups)
        {
            await _repository.CreateListAsync(paymentFollowups.ToList());
            var res = await _repository.SaveChangesAsync();
            return res > 0;

        }

    }
}
