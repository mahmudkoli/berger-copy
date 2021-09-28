using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
    public class CreditLimitCrossNotifictionService : ICreditLimitCrossNotifictionService
    {
        private readonly IRepository<CreditLimitCrossNotification> _repository;
        public CreditLimitCrossNotifictionService(IRepository<CreditLimitCrossNotification> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CreditLimitCrossNotification>> GetTodayCreditLimitCrossNotifiction(IList<string> customer)
        {
            var result = _repository.Where(p => p.NotificationDate.Date == DateTime.Now.Date && customer.Contains(p.CustomerNo)).ToList();
            return result;
        }

        public async Task<bool> SaveMultipleCreditLimitCrossNotifiction(IList<CreditLimitCrossNotification> creditLimits)
        {
            await _repository.CreateListAsync(creditLimits.ToList());
            var res = await _repository.SaveChangesAsync();
            return res > 0;

        }

    }
}
