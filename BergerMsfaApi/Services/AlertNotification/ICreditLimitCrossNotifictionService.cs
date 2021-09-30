using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
    public interface ICreditLimitCrossNotifictionService
    {
        Task<bool> SaveMultipleCreditLimitCrossNotifiction(IList<CreditLimitCrossNotification> creditLimits);
        Task<IEnumerable<CreditLimitCrossNotification>> GetTodayCreditLimitCrossNotifiction(IList<string> customer);
    }
}
