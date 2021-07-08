﻿using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace Berger.Odata.Services
{
    public interface IODataCommonService
    {
        Task<IList<CreditControlArea>> GetAllCreditControlAreasAsync();
        Task<IList<RPRSPolicy>> GetAllRPRSPoliciesAsync();
        Task<IList<(string Code, string Name)>> GetAllDepotsAsync(Func<Depot, bool> predicate);
    }
}
