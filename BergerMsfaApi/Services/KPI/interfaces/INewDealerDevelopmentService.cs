﻿using Berger.Data.MsfaEntity.KPI;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.interfaces
{
    public interface INewDealerDevelopmentService
    {
        Task<IList<NewDealerDevelopment>> GetNewDealerDevelopmentByIdAsync(SearchNewDealerDevelopment query);
        Task<int> AddNewDealerDevelopmentAsync(IList<NewDealerDevelopment> model);
    }
}
