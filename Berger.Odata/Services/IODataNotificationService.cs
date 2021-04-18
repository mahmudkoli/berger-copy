using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IODataNotificationService
    {
        Task<IList<AppChequeBounceNotificationModel>> GetAllTodayCheckBouncesByDealerIds(List<int> dealerIds);
        Task<IList<AppCreditLimitCrossNotificationModel>> GetAllTodayCreditLimitCrossByDealerIds(List<int> dealerIds);
        Task<IList<AppPaymentFollowUpNotificationModel>> GetAllTodayPaymentFollowUpByDealerIds(List<int> dealerIds);
        Task<IList<AppCustomerOccasionNotificationModel>> GetAllTodayCustomerOccasionsByDealerIds(List<int> dealerIds);
    }
}
