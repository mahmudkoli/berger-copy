using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IAlertNotificationDataService
    {
        Task<IList<AppChequeBounceNotificationModel>> GetAllTodayCheckBounces();
        Task<IList<AppCreditLimitCrossNotificationModel>> GetAllTodayCreditLimitCross();
        Task<IList<AppPaymentFollowUpNotificationModel>> GetAllTodayPaymentFollowUp();
        Task<IList<AppCustomerOccasionNotificationModel>> GetAllTodayCustomerOccasions();

    }
}
