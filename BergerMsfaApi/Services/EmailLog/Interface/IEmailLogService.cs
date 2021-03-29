using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.EmailLog;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IEmailLogService
    {
 
        Task<EmailLog> CreateAsync(EmailLog email);
        
    }
}
