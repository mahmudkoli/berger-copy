using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IAlertNotificationDataService
    {
        Task<IList<CollectionDataModel>> GetAllTodayCheckBounces();
        Task<IList<CustomerDataModel>> GetAllTodayCreditLimitCross();
        Task<IList<FinancialDataModel>> GetAllTodayPaymentFollowUp();
        Task<IList<CustomerOccasionDataModel>> GetAllTodayCustomerOccasions();

    }
}
