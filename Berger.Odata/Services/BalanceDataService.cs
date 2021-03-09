using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Model;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class BalanceDataService : IBalanceDataService
    {
        private readonly IODataService _odataService;

        public BalanceDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
        }

        public async Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model)
        {
            return null;
        }
    }
}
