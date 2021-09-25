using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IEmailConfigService
    {
        /// <summary>
        /// Email Config for Dealer Opening Approval
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<EmailConfigForDealerOppening> CreateAsync(EmailConfigForDealerOppening email);
        Task<EmailConfigForDealerOppening> UpdateAsync(EmailConfigForDealerOppening email);
        Task<IEnumerable<EmailConfigForDealerOppening>> GetEmailConfig();
        Task<EmailConfigForDealerOppening> GetById(int id);



        /// <summary>
        /// Email Config For DealerSaleCall
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<EmailConfigForDealerSalesCall> CreateDealerSalesCallAsync(EmailConfigForDealerSalesCall email);
        Task<EmailConfigForDealerSalesCall> UpdateDealerSalesCallAsync(EmailConfigForDealerSalesCall email);
        Task<IEnumerable<EmailConfigForDealerSalesCall>> GetEmailConfigDealerSalesCall();
        Task<EmailConfigForDealerSalesCall> GetByIdDealerSalesCall(int id);
        Task<int> DeleteDealerOppeningEmailById(int id);
        Task<int> DeleteDealerSalesEmailById(int id);
    }
}
