using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Worker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
    public class PaymentFollowupService : IPaymentFollowupService
    {
        private readonly IApplicationRepository<PaymentFollowupNotification> _repository;
        public PaymentFollowupService(IApplicationRepository<PaymentFollowupNotification> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SavePaymentFollowup(IList<PaymentFollowupNotification> paymentFollowups)
        {
          var res=  await _repository.CreateListAsync(paymentFollowups.ToList());
            return res.Count > 0;

        }

    }
}
