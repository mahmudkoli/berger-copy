using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
   public class ChequeBounceNotificationService: IChequeBounceNotificationService
    {
        private readonly IRepository<ChequeBounceNotification> _repository;
        public ChequeBounceNotificationService(IRepository<ChequeBounceNotification> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ChequeBounceNotification>> GetChequeBounceNotification(IList<string> customer)
        {
            var checkBounce = _repository.Where(p => customer.Contains(p.CustomarNo) && p.NotificationDate.Date == DateTime.Now.Date).ToList();

            return checkBounce;
        }

        public async Task<bool> SaveMultipleChequeBounceNotification(IList<ChequeBounceNotification> cheque)
        {
            await _repository.CreateListAsync(cheque.ToList());
            var res =await _repository.SaveChangesAsync();
            return res > 0;

        }
    }
}
