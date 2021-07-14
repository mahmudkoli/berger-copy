using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
   public class ChequeBounceNotificationService: IChequeBounceNotificationService
    {
        private readonly IRepository<ChequeBounceNotification> _repository;
        public ChequeBounceNotificationService(IRepository<ChequeBounceNotification> repository)
        {
            _repository = repository;
        }


        public async Task<bool> SaveMultipleChequeBounceNotification(IList<ChequeBounceNotification> cheque)
        {
            await _repository.CreateListAsync(cheque.ToList());
            var res =await _repository.SaveChangesAsync();
            return res > 0;

        }
    }
}
