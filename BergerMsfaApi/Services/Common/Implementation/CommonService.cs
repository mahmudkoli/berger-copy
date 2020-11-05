using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IRepository<Zone> _zoneSvc;
        private readonly IRepository<Territory> _territorySvc;
        private readonly IRepository<SaleGroup> _saleGroupSvc;
        private readonly IRepository<SaleOffice> _saleOfficeSvc;
        public CommonService(
            IRepository<DealerInfo> dealerInfoSvc,
            IRepository<Zone> zoneSvc,
            IRepository<Territory> territorySvc,
            IRepository<SaleGroup> saleGroupSvc,
            IRepository<SaleOffice> saleOfficeSvc
            )
        {
            _dealerInfoSvc = dealerInfoSvc;
            _zoneSvc = zoneSvc;
            _territorySvc = territorySvc;
            _saleGroupSvc = saleGroupSvc;
            _saleOfficeSvc = saleOfficeSvc;
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

        public async Task<IEnumerable<SaleGroup>> GetSaleGroupList()
        {
            return await _saleGroupSvc.GetAllAsync();
        }

        public  async Task<IEnumerable<SaleOffice>> GetSaleOfficeList()
        {
            return await _saleOfficeSvc.GetAllAsync();
        }

        public async Task<IEnumerable<Territory>> GetTerritoryList()
        {
            return await _territorySvc.GetAllAsync();
        }

        public async Task<IEnumerable<Zone>> GetZoneList()
        {
            return await _zoneSvc.GetAllAsync();
        }
    }
}
