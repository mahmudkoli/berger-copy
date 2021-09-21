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
        private readonly IApplicationRepository<CreditLimitCrossNotifiction> _repository;
        public CreditLimitCrossNotifictionService(IApplicationRepository<CreditLimitCrossNotifiction> repository)
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
