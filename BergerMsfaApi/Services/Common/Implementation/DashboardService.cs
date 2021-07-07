using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepo;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepo;
        private readonly IRepository<Painter> _painterRepo;
        private readonly IRepository<PainterCall> _painterCallRepo;
        private readonly IRepository<DSC.DealerSalesCall> _dealerSalesCallRepo;
        private readonly IAuthService _authService;

        public DashboardService(
            IRepository<LeadGeneration> leadGenerationRepo,
            IRepository<LeadFollowUp> leadFollowUpRepo,
            IRepository<Painter> painterRepo,
            IRepository<PainterCall> PainterCallRepo,
            IRepository<DSC.DealerSalesCall> dealerSalesCallRepo,
            IAuthService authService)
        {
            this._leadGenerationRepo = leadGenerationRepo;
            this._leadFollowUpRepo = leadFollowUpRepo;
            this._painterRepo = painterRepo;
            this._painterCallRepo = PainterCallRepo;
            this._dealerSalesCallRepo = dealerSalesCallRepo;
            this._authService = authService;
        }

        //private async Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesReportsByCurrentUserAsync(int pageIndex, int pageSize, string fmUserIdsStr)
        //{
        //    #region Store Procedure
        //    var storeProcedure = "spGetDCMAReports";
        //    var parameters = new List<(string, object, bool)>
        //    {
        //        ("PageIndex", pageIndex, false),
        //        ("PageSize", pageSize , false),
        //        ("SearchText", "" , false),
        //        ("OrderBy", "DCMA.Date desc", false),
        //        ("FMIds", fmUserIdsStr, false),
        //        ("TotalCount", 0, true),
        //        ("FilteredCount", 0, true)
        //    };

        //    var result = _dailyCMActivityRepo.GetDataBySP<DailyCMTaskReportModel>(storeProcedure, parameters);

        //    return result.Items.ToList();
        //    #endregion
        //}

        public async Task<List<AppDashboardModel>> GetAppDashboardDataAsync()
        {
            var currentUser = AppIdentity.AppUser;
            var result = new List<AppDashboardModel>();
            var model = new AppDashboardParamModel();
            model.EmployeeRole = (EnumEmployeeRole)currentUser.EmployeeRole;
            model.UserId = currentUser.UserId;
            model.Depots = currentUser.PlantIdList;
            model.SalesOffices = currentUser.SalesOfficeIdList;
            model.SalesGroups = currentUser.SalesAreaIdList;
            model.Territories = currentUser.TerritoryIdList;
            model.Zones = currentUser.ZoneIdList;
            var currentDate = DateTime.Now;
            model.FromDate = new DateTime(currentDate.Year, currentDate.Month, 01);
            model.ToDate = currentDate;

            switch (model.EmployeeRole)
            {
                case EnumEmployeeRole.Admin:
                    break;
                case EnumEmployeeRole.GM:
                    break;
                case EnumEmployeeRole.DSM:
                case EnumEmployeeRole.RSM:
                    result.AddRange(await GetDSMRSMDataAsync(model));
                    break;
                case EnumEmployeeRole.BM_BSI:
                    result.AddRange(await GetBMBSIDataAsync(model));
                    break;
                case EnumEmployeeRole.AM:
                case EnumEmployeeRole.TM_TO:
                    result.AddRange(await GetAMTMTODataAsync(model));
                    break;
                case EnumEmployeeRole.ZO:
                    result.AddRange(await GetZODataAsync(model));
                    break;
                default:
                    break;
            }

            return result;
        }

        private async Task<List<AppDashboardModel>> GetZODataAsync(AppDashboardParamModel model)
        {
            var result = new List<AppDashboardModel>();
            result.Add(await GetLeadGenerationCountAsync(model));
            result.Add(await GetPainterCallCountAsync(model));
            result.Add(await GetLeadFollowUpCountAsync(model));
            result.Add(await GetSubDealerSalesCallCountAsync(model));

            return result;
        }

        private async Task<List<AppDashboardModel>> GetAMTMTODataAsync(AppDashboardParamModel model)
        {
            var result = new List<AppDashboardModel>();
            result.Add(await GetTargetAchievementPerAsync(model));
            result.Add(await GetPremiumBrandValueSalesPerAsync(model));
            result.Add(await GetLeadGenerationCountAsync(model));
            result.Add(await GetLeadFollowUpCountAsync(model));
            result.Add(await GetBillingDealerCountAsync(model));

            return result;
        }

        private async Task<List<AppDashboardModel>> GetBMBSIDataAsync(AppDashboardParamModel model)
        {
            var result = new List<AppDashboardModel>();
            result.Add(await GetTargetAchievementPerAsync(model));
            result.Add(await GetPremiumBrandValueSalesPerAsync(model));
            result.Add(await GetLeadGenerationCountAsync(model));
            result.Add(await GetLeadFollowUpCountAsync(model));
            result.Add(await GetBillingDealerCountAsync(model));

            return result;
        }

        private async Task<List<AppDashboardModel>> GetDSMRSMDataAsync(AppDashboardParamModel model)
        {
            var result = new List<AppDashboardModel>();
            result.Add(await GetTargetAchievementPerAsync(model));
            result.Add(await GetPremiumBrandValueSalesPerAsync(model));
            result.Add(await GetLeadGenerationCountAsync(model));
            result.Add(await GetLeadFollowUpCountAsync(model));
            result.Add(await GetBillingDealerCountAsync(model));

            return result;
        }

        private async Task<AppDashboardModel> GetLeadGenerationCountAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Lead Create";
            result.Type = AppDashboardType.Flat;

            Func<LeadGeneration, bool> predicateCommon = x => (x.CreatedTime.Date >= model.FromDate && x.CreatedTime.Date <= model.ToDate);
            Func<LeadGeneration, bool> predicateConditional = GetFuncCount<LeadGeneration>(model, x => x.Depot, x => x.Territory, x => x.Zone);

            Expression<Func<LeadGeneration, bool>> predicateFinal = x => predicateCommon(x) && predicateConditional(x);

            var count = await _leadGenerationRepo.CountFuncAsync(predicateFinal);
            result.Value = count;

            return result;
        }

        private async Task<AppDashboardModel> GetLeadFollowUpCountAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Lead followup";
            result.Type = AppDashboardType.Flat;

            Func<LeadFollowUp, bool> predicateCommon = x => (x.CreatedTime.Date >= model.FromDate && x.CreatedTime.Date <= model.ToDate);
            Func<LeadFollowUp, bool> predicateConditional = GetFuncCount<LeadFollowUp>(model, x => x.LeadGeneration.Depot, x => x.LeadGeneration.Territory, x => x.LeadGeneration.Zone);

            Expression<Func<LeadFollowUp, bool>> predicateFinal = x => predicateCommon(x) && predicateConditional(x);

            var count = await _leadFollowUpRepo.CountFuncAsync(predicateFinal);
            result.Value = count;

            return result;
        }

        private async Task<AppDashboardModel> GetPainterCallCountAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Number of Painter Call";
            result.Type = AppDashboardType.Flat;

            Func<PainterCall, bool> predicateCommon = x => (x.CreatedTime.Date >= model.FromDate && x.CreatedTime.Date <= model.ToDate);
            Func<PainterCall, bool> predicateConditional = GetFuncCount<PainterCall>(model, x => x.Painter.Depot, x => x.Painter.Territory, x => x.Painter.Zone);

            Expression<Func<PainterCall, bool>> predicateFinal = x => predicateCommon(x) && predicateConditional(x);

            var count = await _painterCallRepo.CountFuncAsync(predicateFinal);
            result.Value = count;

            return result;
        }

        private async Task<AppDashboardModel> GetSubDealerSalesCallCountAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Sub-dealer Sales Call";
            result.Type = AppDashboardType.Flat;

            Func<DSC.DealerSalesCall, bool> predicateCommon = x => (x.CreatedTime.Date >= model.FromDate && x.CreatedTime.Date <= model.ToDate) && x.IsSubDealerCall;
            Func<DSC.DealerSalesCall, bool> predicateConditional = GetFuncCount<DSC.DealerSalesCall>(model, x => x.Dealer.BusinessArea, x => x.Dealer.Territory, x => x.Dealer.CustZone);

            Expression<Func<DSC.DealerSalesCall, bool>> predicateFinal = x => predicateCommon(x) && predicateConditional(x);

            var count = await _dealerSalesCallRepo.CountFuncAsync(predicateFinal);
            result.Value = count;

            return result;
        }

        private async Task<AppDashboardModel> GetTargetAchievementPerAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Target Achv %";
            result.Type = AppDashboardType.Percentage;

            return result;
        }

        private async Task<AppDashboardModel> GetBillingDealerCountAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Number of Billing Dealer";
            result.Type = AppDashboardType.Flat;

            return result;
        }

        private async Task<AppDashboardModel> GetPremiumBrandValueSalesPerAsync(AppDashboardParamModel model)
        {
            var result = new AppDashboardModel();
            result.Title = "Premium Brand Value Sales %";
            result.Type = AppDashboardType.Percentage;

            return result;
        }

        private Func<T, bool> GetFuncCount<T>(AppDashboardParamModel model, Func<T, string> depot, Func<T, string> territory, Func<T, string> zone)
        {
            Func<T, bool> predicate = x => true;

            switch (model.EmployeeRole)
            {
                case EnumEmployeeRole.GM:
                    predicate = x => true;
                    break;
                case EnumEmployeeRole.DSM:
                case EnumEmployeeRole.RSM:
                case EnumEmployeeRole.BM_BSI:
                    predicate = x => (!model.Depots.Any() || model.Depots.Contains(depot(x)));
                    break;
                case EnumEmployeeRole.AM:
                case EnumEmployeeRole.TM_TO:
                    predicate = x => (!model.Depots.Any() || model.Depots.Contains(depot(x)))
                                && (!model.Territories.Any() || model.Territories.Contains(territory(x)));
                    break;
                case EnumEmployeeRole.ZO:
                    predicate = x => (!model.Depots.Any() || model.Depots.Contains(depot(x)))
                                && (!model.Territories.Any() || model.Territories.Contains(territory(x)))
                                && (!model.Zones.Any() || model.Zones.Contains(zone(x)));
                    break;
                default:
                    break;
            }

            return predicate;
        }
    }

    internal class AppDashboardParamModel
    {
        public EnumEmployeeRole EmployeeRole { get; set; }
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> Depots { get; set; }
        public List<string> SalesOffices { get; set; }
        public List<string> SalesGroups { get; set; }
        public List<string> Territories { get; set; }
        public List<string> Zones { get; set; }

        public AppDashboardParamModel()
        {
            this.Depots = new List<string>();
            this.SalesOffices = new List<string>();
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }
}
