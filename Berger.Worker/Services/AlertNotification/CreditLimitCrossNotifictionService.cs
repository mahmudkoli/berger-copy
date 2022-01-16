using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Worker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
   public class CreditLimitCrossNotifictionService: ICreditLimitCrossNotifictionService
    {
        private readonly IApplicationRepository<CreditLimitCrossNotification> _repository;
        public CreditLimitCrossNotifictionService(IApplicationRepository<CreditLimitCrossNotification> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SaveMultipleCreditLimitCrossNotifiction(IList<CreditLimitCrossNotification> creditLimits)
        {
          var res=  await _repository.CreateListAsync(creditLimits.ToList());
            return res.Count > 0;

        }

    }
}
