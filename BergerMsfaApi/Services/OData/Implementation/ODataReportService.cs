using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Brand;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using AutoMapper;
using BergerMsfaApi.Models.Common;
using System.Linq.Expressions;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Services.OData.Interfaces;
using Berger.Odata.Services;
using Berger.Odata.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BergerMsfaApi.Services.OData.Implementation
{
    public class ODataReportService : IODataReportService
    {
        private readonly IRepository<BrandInfo> _brandInfoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IMTSDataService _mTSDataService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ISalesDataService _salesDataService;

        public ODataReportService(
            IRepository<BrandInfo> brandInfoRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<DealerInfo> dealerInfoRepository,
            IMTSDataService mTSDataService,
            IMapper mapper,
            ApplicationDbContext context, ISalesDataService salesDataService
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _userInfoRepository = userInfoRepository;
            _dealerInfoRepository = dealerInfoRepository;
            _mTSDataService = mTSDataService;
            _mapper = mapper;
            _context = context;
            _salesDataService = salesDataService;
        }

        public async Task<MySummaryReportResultModel> MySummaryReport()
        {
            var query = await (from master in _context.JourneyPlanMasters
                               join details in _context.JourneyPlanDetails on master.Id equals details.PlanId
                               join dsc in _context.DealerSalesCalls on master.Id equals dsc.JourneyPlanId into dscLeftJoin
                               from dscInfo in dscLeftJoin.DefaultIfEmpty()
                               join painterCall in _context.PainterCalls on master.EmployeeId equals painterCall.EmployeeId into
                                   painterCallLeftJoin
                               from painterCallInfo in painterCallLeftJoin.DefaultIfEmpty()
                               join dsc2 in _context.DealerSalesCalls on new { master.EmployeeId, Date = DateTime.Now.Date }
                                   equals new { EmployeeId = dsc2.UserId.ToString(), Date = dsc2.CreatedTime.Date } into dsc2LeftJoin
                               from dsc2Info in dsc2LeftJoin.DefaultIfEmpty()
                               join ld in _context.LeadGenerations on new { master.EmployeeId, Date = DateTime.Now.Date }
                                   equals new { EmployeeId = ld.UserId.ToString(), Date = ld.CreatedTime.Date } into ldLeftJoin
                               from ldInfo in ldLeftJoin.DefaultIfEmpty()
                               join lfu in _context.LeadFollowUps on new { ldInfo.Id, Date = DateTime.Now.Date }
                                   equals new { Id = lfu.LeadGenerationId, Date = lfu.CreatedTime.Date }
                                   into lfuLeftJoin
                               from lfuInfo in lfuLeftJoin.DefaultIfEmpty()
                               where (master.PlanDate.Date == DateTime.Now.Date)
                               select new
                               {
                                   DelarId = details.Id,
                                   dscInfo.IsSubDealerCall,
                                   PainterCallInfoId = painterCallInfo.Id,
                                   dsc2InfoId = dsc2Info.Id,
                                   LdInfoId = ldInfo.Id,
                                   lfuInfoId = lfuInfo.Id
                               }).ToListAsync();

            return new MySummaryReportResultModel
            {
                DealerVisitTarget = query.Select(x => x.DelarId).Distinct().Count(),
                ActualVisited = query.Count(x => !x.IsSubDealerCall),
                SubDealerActuallyVisited = query.Count(x => x.IsSubDealerCall),
                PainterActuallyVisited = query.Select(x => x.PainterCallInfoId).Distinct().Count(),
                AdHocVisitNo = query.Select(x => x.dsc2InfoId).Distinct().Count(),
                LeadGenerationNo = query.Select(x => x.LdInfoId).Distinct().Count(),
                LeadFollowupNo = query.Select(x => x.lfuInfoId).Distinct().Count()
            };

        }

        public async Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model)
        {
            var customerNoList = new List<int>();

            if (model.ReportType == DealerPerformanceReportType.LastYearAppointed)
            {
                customerNoList= await _dealerInfoRepository
                    .FindByCondition(x => x.IsLastYearAppointed && x.Territory == model.Territory)
                    .Select(x => x.CustomerNo).ToListAsync();
            }
            else
            {
                customerNoList= await _dealerInfoRepository
                    .FindByCondition(x => x.IsClubSupreme && x.Territory == model.Territory)
                    .Select(x => x.CustomerNo).ToListAsync();
            }


            if (!customerNoList.Any())
                return new List<ReportDealerPerformanceResultModel>(); // if no record found in db;


            var result = new List<ReportDealerPerformanceResultModel>
            {
                new ReportDealerPerformanceResultModel()
                {
                    Territory = model.Territory,
                    NumberOfDealer = customerNoList.Count()
                }
            };

            var reportLastYearAppointedNewDealerPerformanceInCurrentYear =
                await _salesDataService.GetReportDealerPerformance(customerNoList, model.ReportType);

            if (reportLastYearAppointedNewDealerPerformanceInCurrentYear.Any())
            {
                reportLastYearAppointedNewDealerPerformanceInCurrentYear.ToList().ForEach(x =>
               {
                   x.NumberOfDealer = customerNoList.Count();
                   x.Territory = model.Territory;
               });
                return reportLastYearAppointedNewDealerPerformanceInCurrentYear;
            }
            else
            {
                return result;
            }
        }
    }
}
