using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.WorkFlows;
using X.PagedList;

namespace BergerMsfaApi.Services.WorkFlows.Interfaces
{
    public interface IWorkFlowConfigurationService
    {
    
        Task<IPagedList<WorkFlowConfigurationModel>> GetPagedQueryWorkFlowConfigurationsAsync(int pageNumber, int pageSize);
       
        Task<WorkFlowConfigurationModel> GetWorkFlowConfigurationAsync(int id);
        Task<WorkFlowConfigurationModel> SaveAsync(WorkFlowConfigurationModel model);
        Task<WorkFlowConfigurationModel> UpdateAsync(WorkFlowConfigurationModel model);
        Task<int> DeleteAsync(int id);

        Task<IEnumerable<WorkFlowConfigurationModel>> GetWorkFlowConfigurationsByWorkflowIdAsync(int id);
        Task<bool> IsSequenceExistAsync(int masterWorkFlowId, int sequence);
        Task<bool> IsOrganizationRoleExistAsync(int masterWorkFlowId, int orgRoleId, int id);
    }
}

