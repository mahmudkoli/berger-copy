using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
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
          var res=  await _repository.CreateListAsync(creditLimits.ToList());
            return res.Count > 0;

        }

    }
}
