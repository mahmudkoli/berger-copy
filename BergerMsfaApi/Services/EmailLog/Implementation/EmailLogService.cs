using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.EmailLog;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class EmailLogService : IEmailLogService
    {
        private readonly IRepository<EmailLog> _emailConfig;


        public EmailLogService(IRepository<EmailLog> emailConfig)
        {
            _emailConfig = emailConfig;
        } 
        public async Task<EmailLog> CreateAsync(EmailLog email)
        {
          var result=await  _emailConfig.CreateAsync(email);
            return email;
        }


    }
}
