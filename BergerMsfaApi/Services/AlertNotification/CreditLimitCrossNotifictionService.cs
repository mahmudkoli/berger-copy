using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Service.AlertNotification
{
   public class CreditLimitCrossNotifictionService: ICreditLimitCrossNotifictionService
    {
        private readonly IRepository<CreditLimitCrossNotifiction> _repository;
        public CreditLimitCrossNotifictionService(IRepository<CreditLimitCrossNotifiction> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SaveMultipleCreditLimitCrossNotifiction(IList<CreditLimitCrossNotifiction> creditLimits)
        {
            await _repository.CreateListAsync(creditLimits.ToList());
            var res = await _repository.SaveChangesAsync();
            return res > 0;

        }

    }
}
