﻿using System;
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
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IODataService _odataService;

        public FinancialDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
        }

        public async Task<IList<CollectionHistoryResultModel>> GetCollectionHistory(CollectionHistorySearchModel model)
        {
            return null;
        }
    }
}
