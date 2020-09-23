using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.WorkFlows;
using X.PagedList;

namespace BergerMsfaApi.Services.WorkFlows.Interfaces
{
    public interface IWorkFlowLogService
    {
        Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogsAsync();
        Task<IPagedList<WorkFlowLogModel>> GetPagedWorkFlowLogsAsync(int pageNumber, int pageSize);
        Task<IPagedList<WorkFlowLogModel>> GetPagedQueryWorkFlowLogsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<WorkFlowLogModel>> GetQueryWorkFlowLogsAsync();
        Task<WorkFlowLogModel> GetWorkFlowLogAsync(int id);
        Task<WorkFlowLogModel> SaveAsync(WorkFlowLogModel model);
        Task<WorkFlowLogModel> CreateAsync(WorkFlowLogModel model);
        Task<WorkFlowLogModel> UpdateAsync(WorkFlowLogModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsWorkFlowLogExistAsync(int id);

        Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogsByWorkflowIdAsync(int id);
        Task<int> GetUnseenHistoryCountByUserId(int id);
        Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogForCurrentUserAsync(int? status, int pageNumber, int pageSize);
    }
}