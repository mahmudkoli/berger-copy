using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.WorkFlows;

namespace BergerMsfaApi.Services.WorkFlows.Interfaces
{
    public interface IWorkFlowService
    {
        Task<IEnumerable<WorkFlowModel>> GetWorkFlowsAsync();
        Task<WorkFlowModel> GetWorkFlowAsync(int id);
        Task<WorkFlowModel> SaveAsync(WorkFlowModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(string code, int id);

        Task<IEnumerable<WorkFlowModel>> GetWorkFlowsWithConfigAsync();

        Task<IEnumerable<WorkflowTypeModel>> GetWorkFlowTypeAsync();
        Task<bool> IsWorkflowTypeExistAsync(int workflowTypeId, int id);
    }
}