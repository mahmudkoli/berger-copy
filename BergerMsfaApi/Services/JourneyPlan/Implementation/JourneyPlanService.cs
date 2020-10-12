using Berger.Data.MsfaEntity;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Setup.Implementation
{
    public class JourneyPlanService : IJourneyService
    {
        private readonly IRepository<JourneyPlan> _journeyPlanService;

        public JourneyPlanService(
            IRepository<JourneyPlan> journeyPlanService
            ) 
            => _journeyPlanService = journeyPlanService;
      
        public async Task<IEnumerable<JourneyPlanModel>> GetJourneyPlanList()
        {
            var result = await _journeyPlanService.GetAllAsync();
            return await result.ToMap<JourneyPlan, JourneyPlanModel>().ToListAsync();

        }
        public async Task<bool> SetApprovedPlan(JourneyPlanModel model)
        {
            var find = await _journeyPlanService.FindAsync(f => f.Id == model.Id);
            if (find != null)
            {
                if (model.ApprovedStatus) find.ApprovedDate = null;
                else find.ApprovedDate = DateTime.Now;
                find.ApprovedBy = AppIdentity.AppUser.UserId;
                await _journeyPlanService.UpdateAsync(find);
                return true;
            }
            return false;
        }
        public async Task<JourneyPlanModel> CreateAsync(JourneyPlanModel model)
        {
            var journeyPlan = model.ToMap<JourneyPlanModel, JourneyPlan>();
            var result = await _journeyPlanService.CreateAsync(journeyPlan);
            return result.ToMap<JourneyPlan, JourneyPlanModel>();
        }
        public async Task<JourneyPlanModel> UpdateAsync(JourneyPlanModel model)
        {
            var journeyPlan = model.ToMap<JourneyPlanModel, JourneyPlan>();
            var result = await _journeyPlanService.UpdateAsync(journeyPlan);
            return result.ToMap<JourneyPlan, JourneyPlanModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _journeyPlanService.DeleteAsync(s => s.Id == id);

        public async Task<bool> IsExistAsync(int id) => await _journeyPlanService.IsExistAsync(f => f.Id == id);

        public async Task<JourneyPlanModel> GetJourneyPlanById(int id)
        {
            var result = await _journeyPlanService.FindAsync(f => f.Id == id);
            return result.ToMap<JourneyPlan, JourneyPlanModel>();
        }
    }
}
