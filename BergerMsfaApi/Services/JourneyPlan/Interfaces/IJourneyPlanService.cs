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
        Task<IPagedList<JourneyPlanDetailModel>> PortalGetJourneyPlanDeailPage(int index, int pageSize, string search );
        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetail();
        Task<IPagedList<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager(int index, int pageSize, string search);
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
        Task<IEnumerable<AppJourneyPlanDetailModel>> AppGetJourneyPlanList(string employeeId);
        Task<List<AppCreateJourneyModel>> AppCreateJourneyPlan(string employeeId,List<AppCreateJourneyModel> model);
        Task<List<AppCreateJourneyModel>> AppUpdateJourneyPlan(string employeeId,List<AppCreateJourneyModel> model);

        //common
        Task<int> DeleteJourneyAsync(int id);
    }

}
