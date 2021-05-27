using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IODataNotificationService
    {
        Task<IList<AppChequeBounceNotificationModel>> GetAllTodayCheckBouncesByDealerIds(List<string> dealerIds);
        Task<IList<AppCreditLimitCrossNotificationModel>> GetAllTodayCreditLimitCrossByDealerIds(List<string> dealerIds);
        Task<IList<AppPaymentFollowUpNotificationModel>> GetAllTodayPaymentFollowUpByDealerIds(List<string> dealerIds);
        Task<IList<AppCustomerOccasionNotificationModel>> GetAllTodayCustomerOccasionsByDealerIds(List<string> dealerIds);
    }
}
