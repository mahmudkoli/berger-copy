using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class EmailConfigService : IEmailConfigService
    {
        private readonly IRepository<EmailConfigForDealerOppening> _emailConfig;
        private readonly IRepository<EmailConfigForDealerSalesCall> _emailConfigDealerSalesCall;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailConfig"></param>
        /// <param name="EmailConfigForDealerOppening"></param>

        public EmailConfigService(IRepository<EmailConfigForDealerOppening> emailConfig, IRepository<EmailConfigForDealerSalesCall> emailConfigDealerSalesCall)
        {
            _emailConfig = emailConfig;
            _emailConfigDealerSalesCall = emailConfigDealerSalesCall;
        } 
        public async Task<EmailConfigForDealerOppening> CreateAsync(EmailConfigForDealerOppening email)
        {
          var result=await  _emailConfig.CreateAsync(email);
            return email;
        }

        public async Task<EmailConfigForDealerOppening> GetById(int id)
        {
            var result = await _emailConfig.FindAsync(p=>p.Id==id);

            return result;
        }

        public async Task<IEnumerable<EmailConfigForDealerOppening>> GetEmailConfig()
        {
           var result= await _emailConfig.GetAllAsync();
            return result;
        }

        public async Task<EmailConfigForDealerOppening> UpdateAsync(EmailConfigForDealerOppening email)
        {
            var res = await _emailConfig.UpdateAsync(email);
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailConfigForDealerSalesCall"></param>
        /// <returns></returns>

        public async Task<EmailConfigForDealerSalesCall> CreateDealerSalesCallAsync(EmailConfigForDealerSalesCall email)
        {
            var result = await _emailConfigDealerSalesCall.CreateAsync(email);
            return email;
        }

        public async Task<EmailConfigForDealerSalesCall> GetByIdDealerSalesCall(int id)
        {
            var result = await _emailConfigDealerSalesCall.FindAsync(p => p.Id == id);

            return result;
        }

        public async Task<IEnumerable<EmailConfigForDealerSalesCall>> GetEmailConfigDealerSalesCall()
        {
            var result = await _emailConfigDealerSalesCall.GetAllAsync();
            return result;
        }

        public async Task<EmailConfigForDealerSalesCall> UpdateDealerSalesCallAsync(EmailConfigForDealerSalesCall email)
        {
            var res = await _emailConfigDealerSalesCall.UpdateAsync(email);
            return res;
        }
    }
}
