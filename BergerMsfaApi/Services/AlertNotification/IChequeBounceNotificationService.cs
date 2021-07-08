using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Service.AlertNotification
{
    public interface IChequeBounceNotificationService
    {
        Task<bool> SaveMultipleChequeBounceNotification(IList<ChequeBounceNotification> cheque);
    }
}
