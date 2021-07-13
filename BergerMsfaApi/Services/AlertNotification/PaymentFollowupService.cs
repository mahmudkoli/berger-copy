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
        private readonly IRepository<PaymentFollowup> _repository;
        public PaymentFollowupService(IRepository<PaymentFollowup> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PaymentFollowup>> GetToayPaymentFollowup(IList<string> customer)
        {
            var res = _repository.Where(p=>p.NotificationDate==DateTime.Now.Date && customer.Contains(p.CustomarNo)).ToList();
            return res;
        }

        public async Task<bool> SavePaymentFollowup(IList<PaymentFollowup> paymentFollowups)
        {
            await _repository.CreateListAsync(paymentFollowups.ToList());
            var res = await _repository.SaveChangesAsync();
            return res > 0;

        }

    }
}
