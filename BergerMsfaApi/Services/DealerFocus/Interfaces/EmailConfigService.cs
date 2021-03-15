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

        public EmailConfigService(IRepository<EmailConfigForDealerOppening> emailConfig)
        {
            _emailConfig = emailConfig;
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
    }
}
