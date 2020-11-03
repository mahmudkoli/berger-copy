using AutoMapper;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        public CommonService(IRepository<DealerInfo> dealerInfoSvc)
        {
            _dealerInfoSvc = dealerInfoSvc;
        }
        //this method expose dealer list by territory for App
        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory)
        {
            var result = await _dealerInfoSvc.FindAllAsync(f=>f.Territory== territory);
           return result.ToMap<DealerInfo, AppDealerInfoModel>();
        }

        public async Task<IEnumerable<DealerInfoModel>> GetDealerInfoList()
        {
            var result = await _dealerInfoSvc.GetAllAsync();
            return result.ToMap<DealerInfo, DealerInfoModel>();
        }
    }
}
