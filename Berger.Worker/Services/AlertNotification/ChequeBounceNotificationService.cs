using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Worker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
    public class ChequeBounceNotificationService : IChequeBounceNotificationService
    {
        private readonly IApplicationRepository<ChequeBounceNotification> _repository;
        public ChequeBounceNotificationService(IApplicationRepository<ChequeBounceNotification> repository)
        {
            _repository = repository;
        }


        public async Task<bool> SaveMultipleChequeBounceNotification(IList<ChequeBounceNotification> cheque)
        {
            var res = await _repository.CreateListAsync(cheque.ToList());
            return res.Count > 0;

        }
    }
}
