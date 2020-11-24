using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Models.Setup;
using X.PagedList;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IJourneyPlanService
    {
        //portal
        //  Task<IEnumerable<JourneyPlanDetailModel>> PortalGetJourneyPlanDetailPage(int index, int pageSize);
        Task<IPagedList<JourneyPlanDetailModel>> PortalGetJourneyPlanDeailPage(int index, int pageSize, string planDate );
        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetail();
        Task<IPagedList<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager(int index, int pageSize, string planDate);
        Task<PortalCreateJouneryModel> PortalGetJourneyPlanById(int Id);

        Task<PortalPlanDetailModel> PortalCreateJourneyPlan(PortalCreateJouneryModel model);
        Task<PortalPlanDetailModel> PortalUpdateJourneyPlan(PortalCreateJouneryModel model);
      

        Task<JourneyPlanDetailModel> GetJourneyPlanDetailById(int PlanId);

        Task<bool> ChangePlanStatus(JourneyPlanStatusChangeModel model);
        Task<bool> CheckAlreadyTodayPlan(DateTime planDate);

        Task<bool> IsExistAsync(int id);



        //App
        Task<IEnumerable<DealerInfoModel>> AppGetJourneyPlanDealerList(string employeeId);
        Task<bool> AppCheckAlreadyTodayPlan(string employeeId, DateTime visitDate);
        Task<IEnumerable<AppJourneyPlanDetailModel>> AppGetJourneyPlanDetailList(string employeeId);
        Task<List<AppCreateJourneyModel>> AppCreateJourneyPlan(List<AppCreateJourneyModel> model);
        Task<List<AppCreateJourneyModel>> AppUpdateJourneyPlan(List<AppCreateJourneyModel> model);

        //common
        Task<int> DeleteJourneyAsync(int id);
    }

}
