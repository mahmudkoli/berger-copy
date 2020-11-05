using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Models.Setup;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IJourneyPlanService
    {
        //portal
        Task<IEnumerable<JourneyPlanDetailModel>> PortalGetJourneyPlanDetailPage(int index, int pageSize);
        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetail();
        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager();
        Task<PortalCreateJouneryModel> PortalGetJourneyPlanById(int Id);

        Task<PortalPlanDetailModel> PortalCreateJourneyPlan(PortalCreateJouneryModel model);
        Task<PortalPlanDetailModel> PortalUpdateJourneyPlan(PortalCreateJouneryModel model);
      

        Task<JourneyPlanDetailModel> GetJourneyPlanDetailById(int PlanId);

        Task<bool> ChangePlanStatus(JourneyPlanStatusChangeModel model);
        Task<bool> CheckAlreadyTodayPlan();

        Task<bool> IsExistAsync(int id);



        //App

        Task<bool> AppCheckAlreadyTodayPlan(int employeeId, DateTime visitDate);
        Task<IEnumerable<AppJourneyPlanDetailModel>> AppGetJourneyPlanDetailList(int employeeId);
        Task<List<AppCreateJourneyModel>> AppCreateJourneyPlan(List<AppCreateJourneyModel> model);
        Task<List<AppCreateJourneyModel>> AppUpdateJourneyPlan(List<AppCreateJourneyModel> model);

        //common
        Task<int> DeleteJourneyAsync(int id);
    }

}
