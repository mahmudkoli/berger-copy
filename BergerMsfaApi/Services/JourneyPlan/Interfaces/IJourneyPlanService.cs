using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Models.Setup;

namespace BergerMsfaApi.Services.Setup.Interfaces
{
    public interface IJourneyService
    {
      
        Task<IEnumerable<JourneyPlanModel>> GetJourneyPlanList();
        Task<JourneyPlanModel> GetJourneyPlanById(int id);

        Task<bool> SetApprovedPlan(JourneyPlanModel model);
        Task<JourneyPlanModel> CreateAsync(JourneyPlanModel model);
        Task<JourneyPlanModel> UpdateAsync(JourneyPlanModel model);
        Task<int> DeleteAsync(int id);


        Task<bool> IsExistAsync(int id);
      
      
     
    }
}
