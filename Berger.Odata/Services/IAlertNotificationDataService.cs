using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IAlertNotificationDataService
    {
        Task<IList<AppChequeBounceNotificationModel>> GetAllTodayCheckBouncesByDealerIds();
        Task<IList<AppCreditLimitCrossNotificationModel>> GetAllTodayCreditLimitCrossByDealerIds();
        Task<IList<AppPaymentFollowUpNotificationModel>> GetAllTodayPaymentFollowUpByDealerIds();
        Task<IList<AppCustomerOccasionNotificationModel>> GetAllTodayCustomerOccasionsByDealerIds();

    }
}
