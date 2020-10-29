using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Models.Setup;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IJourneyPlanService
    {

        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetail();
        Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager();
        Task<PortalCreateJouneryModel> PortalGetJourneyPlanById(int Id);

        Task<PortalPlanDetailModel> PortalCreateJourneyPlan(PortalCreateJouneryModel model);
        Task<PortalPlanDetailModel> PortalUpdateJourneyPlan(PortalCreateJouneryModel model);
        Task<int> DeleteJourneyAsync(int id);

        Task<JourneyPlanDetailModel> GetJourneyPlanDetailById(int PlanId);

        Task<bool> ChangePlanStatus(JourneyPlanStatusChangeModel model);
        Task<bool> CheckAlreadyTodayPlan();
        Task<bool> IsExistAsync(int id);


        //this method for App Service;

        //this method expose journey plan list by employeeId
        Task<IEnumerable<JourneyPlanDetailModel>> AppGetJourneyPlanDetailList(int employeeId);
        Task<PortalPlanDetailModel> AppCreateJourneyPlan(PortalCreateJouneryModel model);
        Task<PortalPlanDetailModel> AppUpdateJourneyPlan(PortalCreateJouneryModel model);
    }

}
