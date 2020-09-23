using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.WorkFlows;

namespace BergerMsfaApi.Services.WorkFlows.Interfaces
{
    public interface IWorkFlowLogHistoryService
    {
        Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoriesAsync();
        Task<WorkFlowLogHistoryModel> GetWorkFlowLogHistoryAsync(int id);
        Task<WorkFlowLogHistoryModel> SaveAsync(WorkFlowLogHistoryModel model);
        Task<WorkFlowLogHistoryModel> CreateAsync(WorkFlowLogHistoryModel model);
        Task<WorkFlowLogHistoryModel> UpdateAsync(WorkFlowLogHistoryModel model);
        Task<int> DeleteAsync(int id);
        
        //Task<bool> IsWorkFlowLogHistoryExistAsync(int id);

        //Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoriesByWorkflowLogIdAsync(int id);
        //Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoriesByUserIdAsync(int id);
        Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoryForCurrentUserAsync(int pageNumber, int pageSize);

    }
}