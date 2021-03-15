using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IEmailConfigService
    {
        Task<EmailConfigForDealerOppening> CreateAsync(EmailConfigForDealerOppening email);
        Task<EmailConfigForDealerOppening> UpdateAsync(EmailConfigForDealerOppening email);
        Task<IEnumerable<EmailConfigForDealerOppening>> GetEmailConfig();
        Task<EmailConfigForDealerOppening> GetById(int id);
    }
}
